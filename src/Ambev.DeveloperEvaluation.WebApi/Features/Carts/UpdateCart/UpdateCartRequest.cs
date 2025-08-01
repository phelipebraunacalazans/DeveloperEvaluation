namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart;

public class UpdateCartRequest
{
    /// <summary>
    /// The unique identifier of the product to update.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets a customer who bought a cart.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets a date of sale.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Gets a branch where the sale was made
    /// </summary>
    public string Branch { get; set; }

    /// <summary>
    /// Gets products in the cart.
    /// </summary>
    public ICollection<UpdateCartItemRequest> Products { get; set; } = [];
}