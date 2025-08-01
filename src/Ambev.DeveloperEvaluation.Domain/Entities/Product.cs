using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Product : BaseEntity
{
    /// <summary>
    /// Initializes a new instance of the Product class.
    /// </summary>
    public Product()
    {
        CreatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the product's name.
    /// Must not be null or empty.
    /// </summary>
    public string Name { get;  private set; }

    /// <summary>
    /// Gets the product's full price.
    /// Must be greater than zero.
    /// </summary>
    public decimal Price { get; private set; }

    /// <summary>
    /// Gets the date and time when the product was created.
    /// </summary>
    public DateTime CreatedAt { get;private set; }

    /// <summary>
    /// Gets the date and time of the last update to the product's information.
    /// </summary>
    public DateTime? UpdatedAt { get; private set; }
    
    /// <summary>
    /// Gets the stock quantity
    /// </summary>
    public int StockQuantity { get; private set; }

    /// <summary>
    /// Change name and price.
    /// </summary>
    /// <param name="name">Name of product.</param>
    /// <param name="price">Price of product.</param>
    public void Update(string name, decimal price, int stockQuantity)
    {
        Name = name;
        Price = price;
        Price = price;
        StockQuantity = stockQuantity;
        UpdatedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Decrease the product quantity in storage.
    /// </summary>
    /// <param name="quantity">Quantity to reduce</param>
    public void DecreaseQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException("Quantity must be positive value to the decrease quantity.");
        }

        StockQuantity -= quantity;

        if (StockQuantity < 0)
        {
            throw new DomainException($"Stock quantity must not be negative #{Id}, current stock: {StockQuantity}, try to decrease {quantity}");
        }

        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Increase the product quantity in storage.
    /// </summary>
    /// <param name="quantity">Quantity to increase</param>
    public void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException("Quantity must be positive value to the increase quantity.");
        }

        StockQuantity += quantity;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Set stock quantity directly.
    /// </summary>
    /// <param name="quantity">Quantity to change.</param>
    /// <exception cref="ArgumentOutOfRangeException">Occurs when try set negative value</exception>
    public void SetStockQuantity(int quantity)
    {
        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must not be negative value to the change quantity.");
        }

        StockQuantity = quantity;
        UpdatedAt = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Performs validation of the user entity using the ProductValidator rules.
    /// </summary>
    /// <returns>
    ///  <see cref="ValidationResultDetail"/> containing:
    /// - IsValid: Indicates whether all validation rules passed
    /// - Errors: Collection of validation errors if any rules failed
    /// </returns>
    public ValidationResultDetail Validate()
    {
        var validator = new ProductValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}