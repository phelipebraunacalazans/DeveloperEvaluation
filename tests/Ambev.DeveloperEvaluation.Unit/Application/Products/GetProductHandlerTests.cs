using Ambev.DeveloperEvaluation.Application.Products;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products;

/// <summary>
/// Contains unit tests for the <see cref="GetProductHandler"/> class.
/// </summary>
public class GetProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly GetProductHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetProductHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public GetProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetProductHandler(_productRepository, _mapper);
    }

    /// <summary>
    /// Tests that an invalid request when try retrieving product details throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid product identifier When get product Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new GetProductCommand(Guid.Empty);

        // When
        var method = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await method.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Tests that product is returned.
    /// </summary>
    [Fact(DisplayName = "Given valid product identifier When get product details Then product is returned")]
    public async Task Given_Product_Details_When_Handle_Result_Should_Return_Product()
    {
        // Given
        var product = ProductTestData.GenerateValid();
        var command = new GetProductCommand(product.Id);

        _mapper.Map<ProductResult>(Arg.Any<Product>()).Returns(new ProductResult
        {
            Id = product.Id,
            Price = product.Price,
            Name = product.Name,
        });
        _productRepository.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult<Product?>(product));

        // When
        var queryResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        queryResult.Should().NotBeNull();
        queryResult.Id.Should().Be(product.Id);
    }
}
