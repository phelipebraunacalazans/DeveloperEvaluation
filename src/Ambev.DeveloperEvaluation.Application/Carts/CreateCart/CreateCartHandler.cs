using Ambev.DeveloperEvaluation.Common.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Domain.Specifications;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartHandler : IRequestHandler<CreateCartCommand, CartResult>
{
    private readonly ICartRepository _cartRepository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly SaleDiscountService _saleDiscountService;
    private readonly SaleRandomNumberGeneratorService _saleNumberGenerator;
    private readonly SaleLimitReachedSpecification _saleLimitReachedSpecification;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="CreateCartHandler"/>.
    /// </summary>
    /// <param name="cartRepository">The cart repository</param>
    /// <param name="userRepository">The user repository</param>
    /// <param name="productRepository">The product repository</param>
    /// <param name="saleDiscountService">Service to apply discounts</param>
    /// <param name="saleNumberGenerator">Service to generate sale number</param>
    /// <param name="saleLimitReachedSpecification">Specification to validate if sale limit was reached</param>
    /// <param name="unitOfWork">Unit of work.</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public CreateCartHandler(
        ICartRepository cartRepository,
        IUserRepository userRepository,
        IProductRepository productRepository,
        SaleDiscountService saleDiscountService,
        SaleRandomNumberGeneratorService saleNumberGenerator,
        SaleLimitReachedSpecification saleLimitReachedSpecification,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _cartRepository = cartRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
        _saleDiscountService = saleDiscountService;
        _saleNumberGenerator = saleNumberGenerator;
        _saleLimitReachedSpecification = saleLimitReachedSpecification;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the <see cref="CreateCartCommand"/> request.
    /// </summary>
    /// <param name="command">The CreateCart command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created cart details</returns>
    public async Task<CartResult> Handle(CreateCartCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateCartValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var loggedUser = await _userRepository.GetByIdAsync(new Guid("c2a03e75-c2e6-40d0-a2f5-105e9610bde6"), cancellationToken);
        if (loggedUser is null || loggedUser.Status is not UserStatus.Active)
        {
            throw new ValidationException(
            [
                new(nameof(command.UserId), "Not found user."),
            ]);
        }

        var customerUser = await _userRepository.GetByIdAsync(command.UserId, cancellationToken);
        if (customerUser is null || customerUser.Status is not UserStatus.Active)
        {
            throw new ValidationException(
            [
                new(nameof(command.UserId), "Not found user."),
            ]);
        }

        Cart cart = new()
        {
            SaleNumber = _saleNumberGenerator.GenerateNext(),
            SoldAt = command.Date,
            StoreName = command.Branch,
            CreatedBy = loggedUser,
            BoughtBy = customerUser,
        };

        var cartItems = await CreateItemsAsync(command, loggedUser, cancellationToken);

        cart.AddItems(cartItems.ToArray());

        if (_saleLimitReachedSpecification.IsSatisfiedBy(cart))
        {
            throw new DomainException("Cannot sell more than 20 items per product.");
        }

        _saleDiscountService.ApplyDiscounts(cart);

        await _cartRepository.CreateAsync(cart);

        await _unitOfWork.ApplyChangesAsync(cancellationToken);

        return _mapper.Map<CartResult>(cart);
    }

    private async Task<IEnumerable<CartItem>> CreateItemsAsync(
        CreateCartCommand command,
        User loggedUser,
        CancellationToken cancellationToken)
    {
        command.Products = command.Products
            .GroupBy(p => p.ProductId)
            .Select(g => new CreateCartItem
            {
                ProductId = g.Key,
                Quantity = g.Sum(_ => _.Quantity),
            })
            .ToArray();

        var productIds = command.Products.Select(p => p.ProductId).ToArray();
        ICollection<Product> products = await _productRepository.ListByIdsAsync(productIds, cancellationToken);

        if (products.Count != productIds.Length)
        {
            var missingProductIds = productIds
                .Where(id => !products.Any(p => p.Id == id))
                .Select(id => id.ToString())
                .Aggregate((id1, id2) => $"#{id1}, #{id2}");

            throw new ValidationException(
            [
                new(nameof(CreateCartItem.ProductId), $"Not found products: {missingProductIds}"),
            ]);
        }

        return
            from p in products
            join c in command.Products on p.Id equals c.ProductId
            select CartItem.CreateForProduct(p, c.Quantity, loggedUser);
    }
}
