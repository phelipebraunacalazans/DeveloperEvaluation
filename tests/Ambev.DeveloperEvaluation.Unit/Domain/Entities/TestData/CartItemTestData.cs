using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class CartItemTestData
{
    private static readonly Faker<CartItem> CardItemFaker = new Faker<CartItem>()
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(c => c.Quantity, f => f.Random.Int(min: 2, max: 20))
        .RuleFor(c => c.Product, (f, ci) => ProductTestData.GenerateValid())
        .RuleFor(c => c.ProductId, (f, ci) => ci.Product.Id)
        .RuleFor(c => c.UnitPrice, (f, ci) => ci.Product.Price)
        .RuleFor(c => c.TotalPreDiscounts, (f, ci) => ci.Quantity * ci.UnitPrice)
        .RuleFor(c => c.TotalAmount, (f, ci) => ci.TotalPreDiscounts - ci.DiscountAmount)
        .RuleFor(c => c.CreatedBy, UserTestData.GenerateValidUser());

    public static CartItem GenerateValid()
    {
        return CardItemFaker.Generate();
    }
}
