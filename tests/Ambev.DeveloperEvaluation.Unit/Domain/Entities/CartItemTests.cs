using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class CartItemTests
{
    [Fact]
    public void When_Try_Create_New_Cart_Item_With_Invalid_Parameters_Should_Be_Throw_Exception()
    {
        // Arrange
        var product = ProductTestData.GenerateValid();
        var customer = UserTestData.GenerateValidUser();

        // Act
        Action tryCreateProductAsNull = () => CartItem.CreateForProduct(null!, 3, customer);
        Action tryCreateUserAsNull = () => CartItem.CreateForProduct(product, 3, null!);
        Action tryCreateWithZeroQuantity = () => CartItem.CreateForProduct(product, 0, customer);
        Action tryCreateWithNegativeQuantity = () => CartItem.CreateForProduct(product, -1, customer);

        // Assert
        tryCreateProductAsNull.Should().Throw<ArgumentNullException>();
        tryCreateUserAsNull.Should().Throw<ArgumentNullException>();
        tryCreateWithZeroQuantity.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("Quantity must be positive value*");
        tryCreateWithNegativeQuantity.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("Quantity must be positive value*");
    }

    [Fact]
    public void When_Create_New_Cart_Item_Totals_Should_Be_Calculated()
    {
        // Arrange
        var product = ProductTestData.GenerateValid();
        var customer = UserTestData.GenerateValidUser();

        // Act
        var cartItem = CartItem.CreateForProduct(product, 3, customer);

        // Assert
        cartItem.UnitPrice.Should().Be(product.Price);
        cartItem.TotalPreDiscounts.Should().Be(product.Price * 3);
        cartItem.TotalAmount.Should().Be(product.Price * 3);
    }

    [Fact]
    public void Given_Cart_Item_When_Apply_Discount_Total_Amount_Should_Be_Reduced_With_Discount()
    {
        // Arrange
        var cartItem = CartItemTestData.GenerateValid();

        // Act
        var originalTotalPreDiscounts = cartItem.TotalPreDiscounts;
        var originalTotalAmount = cartItem.TotalAmount;
        cartItem.ApplyDiscount(10);

        // Assert
        cartItem.TotalAmount.Should().Be(originalTotalAmount - (originalTotalAmount * 0.1M));
        cartItem.TotalPreDiscounts.Should().Be(originalTotalPreDiscounts);
    }

    [Fact]
    public void Given_Cart_Item_When_Calculate_Discount_Result_Should_Be_Percentage_Without_Applied_Into_The_Product()
    {
        // Arrange
        var cartItem = CartItemTestData.GenerateValid();

        // Act
        var originalTotalPreDiscounts = cartItem.TotalPreDiscounts;
        var originalTotalAmount = cartItem.TotalAmount;
        var amountDiscounted = cartItem.CalculateDiscount(10);

        // Assert
        amountDiscounted.Should().Be(originalTotalAmount * 0.1M);
        cartItem.TotalAmount.Should().Be(originalTotalAmount);
        cartItem.TotalPreDiscounts.Should().Be(originalTotalPreDiscounts);
    }

    [Fact]
    public void When_Try_Change_Quantity_With_Invalid_Quantity_Should_Be_Throw_Exception()
    {
        // Arrange
        var cartItem = CartItemTestData.GenerateValid();

        // Act
        Action tryChangeQuantityWithZero = () => cartItem.ChangeQuantity(0);
        Action tryChangeQuantityWithNegative = () => cartItem.ChangeQuantity(-1);

        // Assert
        tryChangeQuantityWithZero.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("New quantity must be positive non zero value*");

        tryChangeQuantityWithNegative.Should()
            .Throw<ArgumentOutOfRangeException>()
            .WithMessage("New quantity must be positive non zero value*");
    }

    [Fact]
    public void When_Change_Quantity_Plus_One_Should_Be_Decrement_Produt_Stock()
    {
        // Arrange
        var cartItem = CartItemTestData.GenerateValid();

        // Act
        var originalProductStock = cartItem.Product.StockQuantity;
        cartItem.ChangeQuantity(cartItem.Quantity + 1);

        // Assert
        cartItem.Product.StockQuantity.Should().Be(originalProductStock - 1);
    }

    [Fact]
    public void When_Change_Quantity_Minus_One_Should_Be_Increment_Produt_Stock()
    {
        // Arrange
        var cartItem = CartItemTestData.GenerateValid();

        // Act
        var originalProductStock = cartItem.Product.StockQuantity;
        cartItem.ChangeQuantity(cartItem.Quantity - 1);

        // Assert
        cartItem.Product.StockQuantity.Should().Be(originalProductStock + 1);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void When_Change_Quantity_Should_Be_Update_Amounts(int quantity)
    {
        // Arrange
        var cartItem = CartItemTestData.GenerateValid();

        // Act
        var originalProductStock = cartItem.Product.StockQuantity;
        var newQuantity = cartItem.Quantity + quantity;
        cartItem.ChangeQuantity(newQuantity);

        // Assert
        cartItem.Quantity.Should().Be(newQuantity);
        cartItem.UpdatedAt.Should().NotBeNull();
        cartItem.TotalPreDiscounts.Should().Be(cartItem.Product.Price * newQuantity);
        cartItem.TotalAmount.Should().Be(cartItem.Product.Price * newQuantity);
    }
}
