using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart;

public class CreateCartCommand : IRequest<CartResult>
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
    public ICollection<CreateCartItem> Products { get; set; } = [];

    public ValidationResultDetail Validate()
    {
        var validator = new CreateCartValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}