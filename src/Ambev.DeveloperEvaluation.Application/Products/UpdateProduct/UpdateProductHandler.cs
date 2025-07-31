using Ambev.DeveloperEvaluation.Common.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of <see cref="UpdateProductHandler"/>.
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="unitOfWork">Unit of work</param>
    /// <param name="mapper">The AutoMapper instance</param>
    public UpdateProductHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the <see cref="UpdateProductCommand"/> request.
    /// </summary>
    /// <param name="command">The UpdateProduct command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated product details</returns>
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateProductValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var product = await _productRepository.GetByIdAsync(command.Id, cancellationToken);
        if (product is null)
            throw new InvalidOperationException($"Product with id {command.Id} already exists");

        var existingProduct = await _productRepository.GetByNameAsync(command.Name, cancellationToken);
        if (existingProduct is not null && existingProduct.Id != command.Id)
            throw new InvalidOperationException($"Product with name {command.Name} already exists");

        product.Update(command.Name, command.Price);

        await _unitOfWork.ApplyChangesAsync(cancellationToken);
        return _mapper.Map<UpdateProductResult>(product);
    }
}
