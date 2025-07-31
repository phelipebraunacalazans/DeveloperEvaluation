using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class CartTests
{
    [Fact]
    public void Given_Cart_When_Add_One_Items_Should_Have_One()
    {
        // Arrange
        var cart = CartTestData.GenerateValid();
        var cartItem = CartItemTestData.GenerateValid();

        // Act
        cart.AddItems(cartItem);

        // Assert
        cart.Items.Should().HaveCount(1);
        cart.TotalSaleAmount.Should().Be(cartItem.Quantity * cartItem.UnitPrice);
    }

    [Fact]
    public void Given_Cart_When_Add_Two_Items_Should_Have_Two()
    {
        // Arrange
        var cart = CartTestData.GenerateValid();
        var cartItem1 = CartItemTestData.GenerateValid();
        var cartItem2 = CartItemTestData.GenerateValid();

        // Act
        cart.AddItems(cartItem1, cartItem2);

        // Assert
        cart.Items.Should().HaveCount(2);
        cart.TotalSaleAmount.Should().Be((cartItem1.Quantity * cartItem1.UnitPrice) + (cartItem2.Quantity * cartItem2.UnitPrice));
    }

    [Fact]
    public void Given_Cart_When_Add_Items_Stock_Should_Be_Decremented()
    {
        // Arrange
        var cart = CartTestData.GenerateValid();
        var cartItem = CartItemTestData.GenerateValid();

        // Act
        var originalStock = cartItem.Product.StockQuantity;
        cart.AddItems(cartItem);

        // Assert
        cartItem.Product.StockQuantity.Should().Be(originalStock - cartItem.Quantity);
    }

    [Fact]
    public void Given_Cart_With_Item_When_Cancel_Purchase_Status_Should_Be_Cancelled()
    {
        // Arrange
        var cart = CartTestData.GenerateValid();
        var cartItem = CartItemTestData.GenerateValid();
        cart.AddItems(cartItem);

        // Act
        cart.Cancel(cart.BoughtBy);

        // Assert
        cartItem.PurchaseStatus.Should().Be(PurchaseStatus.Cancelled);
        cartItem.CancelledBy.Should().Be(cart.BoughtBy);
        cartItem.CancelledAt.Should().NotBeNull();
        cartItem.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Given_Cart_With_Item_When_Cancel_Product_Stock_Should_Be_Returned()
    {
        // Arrange
        var cart = CartTestData.GenerateValid();
        var cartItem = CartItemTestData.GenerateValid();
        cart.AddItems(cartItem);

        // Act
        var originalStock = cartItem.Product.StockQuantity;
        cart.Cancel(cart.BoughtBy);

        // Assert
        cartItem.Product.StockQuantity.Should().Be(originalStock + cartItem.Quantity);
        cartItem.Product.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Given_Cart_When_Try_Change_With_Null_User_Should_Be_Modified()
    {
        // Arrange
        var customer = UserTestData.GenerateValidUser();
        var cart = CartTestData.GenerateValid();
        var cartItem = CartItemTestData.GenerateValid();
        cart.AddItems(cartItem);

        // Act
        Action method = () => cart.Change(null!, DateTime.UtcNow, "New branch store");

        // Assert
        method.Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void Given_Cart_When_Change_Product_Infos_Should_Be_Modified()
    {
        // Arrange
        var customer = UserTestData.GenerateValidUser();
        var cart = CartTestData.GenerateValid();
        var cartItem = CartItemTestData.GenerateValid();
        cart.AddItems(cartItem);

        // Act
        cart.Change(customer, DateTime.UtcNow, "New branch store");

        // Assert
        cart.StoreName.Should().Be("New branch store");
        cart.SoldAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        cart.BoughtBy.Should().Be(customer);
        cart.BoughtById.Should().Be(customer.Id);
        cart.UpdatedAt.Should().NotBeNull();
    }

    [Fact]
    public void Given_Cart_When_Try_Delete_With_Null_User_Should_Be_Modified()
    {
        // Arrange
        var cart = CartTestData.GenerateValid();

        // Act
        Action method = () => cart.Delete(null!);

        // Assert
        method.Should()
            .Throw<ArgumentNullException>();
    }

    [Fact]
    public void Given_Cart_When_Delete_Purchase_Status_Should_Be_Deleted()
    {
        // Arrange
        var customer = UserTestData.GenerateValidUser();
        var cart = CartTestData.GenerateValid();
        var cartItem = CartItemTestData.GenerateValid();
        cart.AddItems(cartItem);

        // Act
        cart.Delete(customer);

        // Assert
        cart.CancelledAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        cart.CancelledBy.Should().Be(customer);
        cart.PurchaseStatus.Should().Be(PurchaseStatus.Deleted);
        cart.DeletedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        cart.DeletedBy.Should().Be(customer);
        cart.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Given_Cart_When_Delete_Item_Quantity_Should_Be_Returned_To_Product_Stock()
    {
        // Arrange
        var cart = CartTestData.GenerateValid();
        var cartItem = CartItemTestData.GenerateValid();
        cart.AddItems(cartItem);

        // Act
        var originalStock = cartItem.Product.StockQuantity;
        cart.Delete(cart.BoughtBy);

        // Assert
        cartItem.Product.StockQuantity.Should().Be(originalStock + cartItem.Quantity);
    }

    [Fact]
    public void Given_Cart_With_Items_When_Refresh_Total_Amount_Should_Be_Updated()
    {
        // Arrange
        var cart = CartTestData.GenerateValid();
        var cartItem = CartItemTestData.GenerateValid();
        cart.Items.Add(cartItem);

        // Act
        cart.RefreshTotalAmount();

        // Assert
        cart.TotalSaleAmount.Should().Be(cartItem.UnitPrice * cartItem.Quantity);
    }

    [Fact]
    public void Given_Cart_With_Three_Items_When_Delete_Items_Total_Should_Be_Updated()
    {
        // Arrange
        var cart = CartTestData.GenerateValid();
        var cartItem1 = CartItemTestData.GenerateValid();
        var cartItem2 = CartItemTestData.GenerateValid();
        var cartItem3 = CartItemTestData.GenerateValid();
        cart.AddItems(cartItem1, cartItem2, cartItem3);

        // Act
        cart.DeleteItems(cart.BoughtBy, cartItem1, cartItem2);

        // Assert
        cart.Items.Should().HaveCount(3);
        cart.TotalSaleAmount.Should().Be(cartItem3.UnitPrice * cartItem3.Quantity);

        cartItem1.PurchaseStatus.Should().Be(PurchaseStatus.Deleted);
        cartItem2.PurchaseStatus.Should().Be(PurchaseStatus.Deleted);
        cartItem3.PurchaseStatus.Should().Be(PurchaseStatus.Created);
    }

    [Fact]
    public void Given_Cart_When_Delete_Items_Quantity_Should_Be_Returned_To_Product_Stock()
    {
        // Arrange
        var cart = CartTestData.GenerateValid();
        var cartItem1 = CartItemTestData.GenerateValid();
        var cartItem2 = CartItemTestData.GenerateValid();
        var cartItem3 = CartItemTestData.GenerateValid();
        cart.AddItems(cartItem1, cartItem2, cartItem3);

        // Act
        var originalStock1 = cartItem1.Product.StockQuantity;
        var originalStock2 = cartItem2.Product.StockQuantity;
        var originalStock3 = cartItem3.Product.StockQuantity;

        cart.DeleteItems(cart.BoughtBy, cartItem1, cartItem2);

        // Assert
        cartItem1.Product.StockQuantity.Should().Be(originalStock1 + cartItem1.Quantity);
        cartItem2.Product.StockQuantity.Should().Be(originalStock2 + cartItem2.Quantity);
        cartItem3.Product.StockQuantity.Should().Be(originalStock3);
    }

    [Fact]
    public void Given_Cart_With_Three_Items_When_Delete_Removed_Items_Should_Be_Deleted()
    {
        // Arrange
        var cart = CartTestData.GenerateValid();
        var cartItem1 = CartItemTestData.GenerateValid();
        var cartItem2 = CartItemTestData.GenerateValid();
        var cartItem3 = CartItemTestData.GenerateValid();
        cart.AddItems(cartItem1, cartItem2, cartItem3);

        // Act
        cart.DeleteItems(cart.BoughtBy, cartItem1, cartItem2);

        // Assert
        cartItem1.PurchaseStatus.Should().Be(PurchaseStatus.Deleted);
        cartItem2.PurchaseStatus.Should().Be(PurchaseStatus.Deleted);
        cartItem3.PurchaseStatus.Should().Be(PurchaseStatus.Created);
    }

    [Fact]
    public void Given_Cart_With_Three_Items_When_Change_Quantities_Items_Should_Be_Updated()
    {
        // Arrange
        var cart = CartTestData.GenerateValid();
        var cartItem1 = CartItemTestData.GenerateValid();
        var cartItem2 = CartItemTestData.GenerateValid();
        var cartItem3 = CartItemTestData.GenerateValid();
        cart.AddItems(cartItem1, cartItem2, cartItem3);

        // Act
        var originalQuantityItem1 = cartItem1.Quantity;
        var originalQuantityItem2 = cartItem2.Quantity;

        var originalStockProduct1 = cartItem1.Product.StockQuantity;
        var originalStockProduct2 = cartItem2.Product.StockQuantity;

        var changedItem1 = CartItem.CreateForProduct(cartItem1.Product, cartItem1.Quantity - 1, cart.CreatedBy);
        var changedItem2 = CartItem.CreateForProduct(cartItem2.Product, cartItem2.Quantity + 1, cart.CreatedBy);

        cart.ChangeQuantities(changedItem1, changedItem2);

        // Assert
        cart.Items.Should().HaveCount(3);
        cartItem1.PurchaseStatus.Should().Be(PurchaseStatus.Created);
        cartItem2.PurchaseStatus.Should().Be(PurchaseStatus.Created);
        cartItem3.PurchaseStatus.Should().Be(PurchaseStatus.Created);

        cartItem1.Quantity.Should().Be(originalQuantityItem1 - 1);
        cartItem2.Quantity.Should().Be(originalQuantityItem2 + 1);

        cartItem1.Product.StockQuantity.Should().Be(originalStockProduct1 + 1);
        cartItem2.Product.StockQuantity.Should().Be(originalStockProduct2 - 1);

        cart.TotalSaleAmount.Should().Be(cart.Items.Sum(i => i.UnitPrice * i.Quantity));
    }
}
