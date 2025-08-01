namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

public class CreateCartRequest
{
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
    public ICollection<CreateCartItemRequest> Products { get; set; } = [];
}