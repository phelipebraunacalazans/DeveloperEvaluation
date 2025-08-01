namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

public class CartResponse
{
    /// <summary>
    /// The unique identifier of the cart.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Gets a customer who bought a cart.
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    /// Gets a date of sale.
    /// </summary>
    public DateTime Date { get; init; }

    /// <summary>
    /// Gets a branch where the sale was made.
    /// </summary>
    public required string Branch { get; init; }

    /// <summary>
    /// Gets a sale number.
    /// </summary>
    public long SaleNumber { get; init; }

    /// <summary>
    /// Gets the total a sale amount.
    /// </summary>
    public decimal TotalSaleAmount { get; init; }

    /// <summary>
    /// Gets a date of sale.
    /// </summary>
    public DateTime SoldAt { get; init; }

    /// <summary>
    /// Indicates if cart is cancelled.
    /// </summary>
    /// <remarks>
    /// True is cancelled, false otherwise.
    /// </remarks>
    public bool IsCancelled { get; init; }

    /// <summary>
    /// Gets products in the cart.
    /// </summary>
    public required ICollection<CartItemResponse> Products { get; init; }
}