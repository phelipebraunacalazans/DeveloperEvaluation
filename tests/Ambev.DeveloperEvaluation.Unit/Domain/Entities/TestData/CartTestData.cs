using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class CartTestData
{
    private static readonly Faker<Cart> CartFaker = new Faker<Cart>()
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(c => c.StoreName, f => f.Name.JobArea())
        .RuleFor(c => c.SoldAt, f => f.Date.Between(DateTime.UtcNow, DateTime.UtcNow.AddHours(2)))
        .RuleFor(c => c.SaleNumber, f => f.Random.Long(min: 100000000, max: 999999999))
        .RuleFor(c => c.CreatedBy, f => UserTestData.GenerateValidUser())
        .RuleFor(c => c.BoughtBy, f => UserTestData.GenerateValidUser())
        .RuleFor(c => c.BoughtById, (f, c) => c.BoughtBy.Id);

    public static Cart GenerateValid()
    {
        return CartFaker.Generate();
    }
}