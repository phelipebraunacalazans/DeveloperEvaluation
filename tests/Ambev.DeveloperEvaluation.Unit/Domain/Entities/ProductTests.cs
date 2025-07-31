using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class ProductTests
{
    [Fact(DisplayName = "Product updated at should change not be null when change values")]
    public void Given_Product_When_Change_Info_Then_UpdatedAt_Should_Not_Be_Null()
    {
        // Arrange
        var product = ProductTestData.GenerateValid();

        // Act
        product.Update("Modified name", 1, 10);

        // Assert
        product.Name.Should().Be("Modified name");
        product.Price.Should().Be(1);
        product.UpdatedAt.Should().NotBeNull();
    }

    /// <summary>
    /// Tests that validation fails when product properties are invalid.
    /// </summary>
    [Fact(DisplayName = "Validation should fail for invalid product data")]
    public void Given_InvalidProductData_When_Validated_Then_ShouldReturnInvalid()
    {
        // Arrange
        var product = new Product();

        // Act
        var result = product.Validate();

        // Assert
        Assert.False(result.IsValid);
        Assert.NotEmpty(result.Errors);
    }
}
