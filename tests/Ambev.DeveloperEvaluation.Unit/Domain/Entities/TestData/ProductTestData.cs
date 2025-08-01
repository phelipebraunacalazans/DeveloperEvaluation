using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class ProductTestData
{
    private static readonly Faker<Product> ProductFaker = new Faker<Product>()
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(u => u.Name, f => f.Lorem.Sentence(4))
        .RuleFor(u => u.Price, f => f.Random.Decimal(min: 1, max: 2000.00M))
        .RuleFor(u => u.StockQuantity, f => f.Random.Int(min: 200, max: 1000));

    public static Product GenerateValid()
    {
        return ProductFaker.Generate();
    }
}