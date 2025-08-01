namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart;

public class DeleteCartRequest
{
    /// <summary>
    /// The unique identifier of the cart to delete
    /// </summary>
    public Guid Id { get; set; }
}