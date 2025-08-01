using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebIntegrationTesting.Data;
using Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework.Seeding;
using Ambev.DeveloperEvaluation.WebIntegrationTesting.Extensions;
using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.Application;

/// <summary>
/// Representa um semeador de dados.
/// </summary>
/// <param name="instances">Contexto de dados</param>
public class WebSeeder(IDataContext instances)
  : ISeedable
{
    private readonly IDataContext _instances = instances ?? throw new ArgumentNullException(nameof(instances));

    private static readonly Faker<User> UserFaker = new Faker<User>()
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(u => u.Username, f => f.Internet.UserName())
        .RuleFor(u => u.Password, f => $"Test@{f.Random.Number(100, 999)}")
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Phone, f => $"+55{f.Random.Number(11, 99)}{f.Random.Number(100000000, 999999999)}")
        .RuleFor(u => u.Status, f => f.PickRandom(UserStatus.Active, UserStatus.Suspended))
        .RuleFor(u => u.Role, f => f.PickRandom(UserRole.Customer, UserRole.Admin));

    private static readonly Faker<Product> ProductFaker = new Faker<Product>()
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(u => u.Name, f => f.Lorem.Sentence(4))
        .RuleFor(u => u.Price, f => f.Random.Decimal(min: 1, max: 2000.00M))
        .RuleFor(u => u.StockQuantity, f => f.Random.Int(min: 200, max: 1000));

    private static readonly Faker<Cart> CartFaker = new Faker<Cart>()
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(c => c.StoreName, f => f.Name.JobArea())
        .RuleFor(c => c.SoldAt, f => f.Date.Between(DateTime.UtcNow, DateTime.UtcNow.AddHours(2)))
        .RuleFor(c => c.SaleNumber, f => f.Random.Long(min: 100000000, max: 999999999))
        .RuleFor(c => c.CreatedBy, f => UserFaker.Generate())
        .RuleFor(c => c.BoughtBy, f => UserFaker.Generate())
        .RuleFor(c => c.BoughtById, (f, c) => c.BoughtBy.Id);

    private static readonly Faker<CartItem> CardItemFaker = new Faker<CartItem>()
        .RuleFor(c => c.Id, f => f.Random.Guid())
        .RuleFor(c => c.Quantity, f => f.Random.Int(min: 2, max: 20))
        .RuleFor(c => c.Product, (f, ci) => ProductFaker.Generate())
        .RuleFor(c => c.ProductId, (f, ci) => ci.Product.Id)
        .RuleFor(c => c.UnitPrice, (f, ci) => ci.Product.Price)
        .RuleFor(c => c.TotalPreDiscounts, (f, ci) => ci.Quantity * ci.UnitPrice)
        .RuleFor(c => c.TotalAmount, (f, ci) => ci.TotalPreDiscounts - ci.DiscountAmount)
        .RuleFor(c => c.CreatedBy, UserFaker.Generate());

    public WebSeeder NewCart(Guid? id = null)
    {
        var card = CartFaker.Generate();

        if (id.HasValue)
        {
            card.WithId(id);
        }

        _instances.Add(card);

        return this;
    }

    public WebSeeder AddItem(Guid? id = null)
    {
        var cardItem = CardItemFaker.Generate();

        if (id.HasValue)
        {
            cardItem.WithId(id);
        }

        _instances.Add(cardItem);

        return this;
    }

    public WebSeeder NewManyCarts(int count)
    {
        _instances.AddRange(CartFaker.Generate(count).ToArray());
        return this;
    }

    public WebSeeder AddManyItems(int count)
    {
        _instances.AddRange(CardItemFaker.Generate(count));
        return this;
    }

    public WebSeeder NewCustomer(Guid? id = null)
    {
        var user = UserFaker.Generate();
        user.Status = UserStatus.Active;

        if (id.HasValue)
        {
            user.WithId(id);
        }

        _instances.Add(user);

        return this;
    }

    public WebSeeder UserSuspended()
    {
        var user = _instances.Last<User>();
        user.Status = UserStatus.Suspended;
        return this;
    }

    public WebSeeder NewProduct(Guid? id = null)
    {
        var product = ProductFaker.Generate();
        
        if (id.HasValue)
        {
            product.WithId(id);
        }

        _instances.Add(product);

        return this;
    }
}
