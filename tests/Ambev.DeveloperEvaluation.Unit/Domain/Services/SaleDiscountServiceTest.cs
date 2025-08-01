using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Services;

public class SaleDiscountServiceTest
{
    [Theory]
    [InlineData(1, 0)]
    [InlineData(3, 0)]
    [InlineData(4, 10)]
    [InlineData(9, 10)]
    [InlineData(10, 20)]
    [InlineData(20, 20)]
    [InlineData(21, 0)]
    public void Given_Cart_With_One_Item_When_Apply_Discount_Item_Amount_Should_Be_Calculated(
        int quantity,
        decimal expectedDiscountPercentage)
    {
        // Arrange
        var customer = UserTestData.GenerateValidUser();
        var product = ProductTestData.GenerateValid();
        var cart = CartTestData.GenerateValid();
        var cartItem = CartItem.CreateForProduct(product, quantity, customer);
        cart.AddItems(cartItem);
        var total = cartItem.UnitPrice * quantity;
        var discountService = new SaleDiscountService();

        // Act
        discountService.ApplyDiscounts(cart);

        // Assert
        cartItem.DiscountPercentage.Should().Be(expectedDiscountPercentage);
        cartItem.DiscountAmount.Should().Be(total * (expectedDiscountPercentage / 100));
        cartItem.TotalPreDiscounts.Should().Be(total);
        cartItem.TotalAmount.Should().Be(total - (total * (expectedDiscountPercentage/100)));
        cart.TotalSaleAmount.Should().Be(total - (total * (expectedDiscountPercentage / 100)));
    }
}
