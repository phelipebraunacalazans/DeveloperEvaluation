using Ambev.DeveloperEvaluation.Application.Products;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Common.Interfaces.Repositories;
using Ambev.DeveloperEvaluation.Common.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Unit.Application.Products.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the <see cref="CreateProductHandler"/> class.
/// </summary>
public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly CreateProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateProductHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _mapper = Substitute.For<IMapper>();
        _handler = new CreateProductHandler(
            _productRepository,
            _unitOfWork,
            _mapper);
    }

    /// <summary>
    /// Tests that an invalid request should be return validation errors.
    /// </summary>
    [Fact(DisplayName = "Given invalid command When validate Then result validation errors")]
    public void Handle_InvalidRequest_ReturnValidationError()
    {
        // Given
        var command = new CreateProductCommand();

        // When
        var validationResult = command.Validate();

        // Then
        validationResult.Should().NotBeNull();
        validationResult.Errors.Should().NotBeEmpty();
    }

    /// <summary>
    /// Tests that an invalid request when try to create product throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid request When try create product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateProductCommand();

        // When
        var method = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await method.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that an valid request when delete product should return true to indicates a success operation.
    /// </summary>
    [Fact(DisplayName = "Given valid product identifier When to create a product Then should return true to indicates a success operation")]
    public async Task Handle_ValidRequest_And_Exists_Category_Should_Returns_Success()
    {
        // Given
        var productId = Guid.NewGuid();
        var command = CreateProductHandlerTestData.GenerateValidCommand();

        _unitOfWork.ApplyChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));
        _mapper.Map<Product>(Arg.Any<CreateProductCommand>())
            .Returns(CreateProductHandlerTestData.GenerateProductByCommand(command));
        _mapper.Map<ProductResult>(Arg.Any<Product>())
            .Returns(new ProductResult
            {
                Id = productId,
                Price = command.Price,
                Name = command.Name,
            });

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(productId);
    }

    /// <summary>
    /// Tests that an valid request when delete product should return true to indicates a success operation.
    /// </summary>
    [Fact(DisplayName = "Given valid product identifier When to create a product Then should return true to indicates a success operation")]
    public async Task Handle_ValidRequest_And_Not_Exists_Category_Should_Returns_Success()
    {
        // Given
        var productId = Guid.NewGuid();
        var command = CreateProductHandlerTestData.GenerateValidCommand();

        _unitOfWork.ApplyChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));
        _mapper.Map<Product>(Arg.Any<CreateProductCommand>())
            .Returns(CreateProductHandlerTestData.GenerateProductByCommand(command));
        _mapper.Map<ProductResult>(Arg.Any<Product>())
            .Returns(new ProductResult
            {
                Id = productId,
                Price = command.Price,
                Name = command.Name,
            });

        // When
        var result = await _handler.Handle(command, CancellationToken.None);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(productId);
    }
}
