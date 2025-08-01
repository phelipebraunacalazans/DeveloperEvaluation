using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Products.TestData;

/// <summary>
/// Provides methods for generating test data using the Bogus library.
/// This class centralizes all test data generation to ensure consistency
/// across test cases and provide both valid and invalid data scenarios.
/// </summary>
public static class CreateProductHandlerTestData
{
    /// <summary>
    /// Configures the Faker to generate valid product command.
    /// The generated users will have valid:
    /// - Username (using internet usernames)
    /// - Password (meeting complexity requirements)
    /// - Email (valid format)
    /// - Phone (Brazilian format)
    /// - Status (Active or Suspended)
    /// - Role (Customer or Admin)
    /// </summary>
    private static readonly Faker<CreateProductCommand> createProductHandlerFaker = new Faker<CreateProductCommand>()
        .RuleFor(u => u.Name, f => f.Lorem.Sentence(4))
        .RuleFor(u => u.Price, f => f.Random.Decimal(min: 1, max: 2000.00M))
        .RuleFor(u => u.StockQuantity, f => f.Random.Int(min: 200, max: 1000));

    /// <summary>
    /// Generates a valid Product entity with randomized data.
    /// The generated product will have all properties populated with valid values
    /// that meet the system's validation requirements.
    /// </summary>
    /// <returns>A valid Product command with randomly generated data.</returns>
    public static CreateProductCommand GenerateValidCommand()
    {
        return createProductHandlerFaker.Generate();
    }

    public static Product GenerateProductByCommand(CreateProductCommand command)
    {
        var faker = new Faker<Product>()
            .RuleFor(u => u.Name, command.Name)
            .RuleFor(u => u.Price, command.Price)
            .RuleFor(u => u.StockQuantity, command.StockQuantity);
        return faker.Generate(1).First();
    }
}
