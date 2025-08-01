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

    [Fact]
    public void Given_Product_When_Try_Set_Stock_Quantity_To_Negative_Should_Throw_Exception()
    {
        // Arrange
        var product = ProductTestData.GenerateValid();

        // Act
        Action action = () => product.SetStockQuantity(-1);

        // Assert
        action.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("Quantity must not be negative value to the change quantity.*");
    }

    [Fact]
    public void Given_Product_When_Set_Stock_Quantity_Then_Stock_Quantity_And_Update_Date_Should_Be_Changed()
    {
        // Arrange
        var product = ProductTestData.GenerateValid();

        // Act
        product.SetStockQuantity(666);

        //Assert
        product.StockQuantity.Should().Be(666);
        product.UpdatedAt.Should().NotBeNull();
    }

   

    [Fact]
    public void Given_Product_When_Decrease_All_Quantities_Stock_Should_Be_Zero()
    {
        // Arrange
        var product = ProductTestData.GenerateValid();

        // Act
        product.DecreaseQuantity(product.StockQuantity);

        // Assert
        product.StockQuantity.Should().Be(0);
        product.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Given_Product_When_Increase_Quantity_Stock_Should_Be_Changed()
    {
        // Arrange
        var product = ProductTestData.GenerateValid();

        // Act
        var originalQuantity = product.StockQuantity;
        product.IncreaseQuantity(1);

        // Assert
        product.StockQuantity.Should().Be(originalQuantity + 1);
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
