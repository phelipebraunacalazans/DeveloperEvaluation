namespace Ambev.DeveloperEvaluation.Application.Carts;

public class CartItemResult
{
    /// <summary>
    /// The unique identifier of the item cart.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets the product identifier.
    /// </summary>
    public Guid ProductId { get; init; }

    /// <summary>
    /// Gets the quantity of products.
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// Gets the unit price of product.
    /// </summary>
    public decimal UnitPrice { get; init; }

    /// <summary>
    /// Gets the total discount percentage applied.
    /// </summary>
    public decimal DiscountPercentage { get; init; }

    /// <summary>
    /// Gets the discount amount.
    /// </summary>
    public decimal DiscountAmount { get; init; }

    /// <summary>
    /// Gets the total amount.
    /// </summary>
    public decimal TotalAmount { get; init; }

    /// <summary>
    /// Indicates if item is cancelled.
    /// </summary>
    /// <remarks>
    /// True is cancelled, false otherwise.
    /// </remarks>
    public bool IsCancelled { get; init; }
}
