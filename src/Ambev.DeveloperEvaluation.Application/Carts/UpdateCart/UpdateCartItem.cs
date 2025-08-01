namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

public class UpdateCartItem
{
    /// <summary>
    /// Gets the product identifier.
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets the quantity of products.
    /// </summary>
    public int Quantity { get; set; }
}