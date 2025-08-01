using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart;

public class UpdateCartValidator : AbstractValidator<UpdateCartCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateCartValidator"/> with defined validation rules.
    /// </summary>
    /// <remarks>
    /// Validation rules include:
    /// <listheader>The validation includes checking:</listheader
    /// <list type="bullet">Id: Required, must not be emtpy</list>
    /// <list type="bullet">UserId: Required, must be not empty</list>
    /// <list type="bullet">Date: Required, must be not empty</list>
    /// <list type="bullet">Products: Required, must be not empty</list>
    /// <list type="bullet">Branch: Required, must be between 2 and 100 characters</list>
    /// </remarks>
    public UpdateCartValidator()
    {
        RuleFor(cart => cart.Id).NotEmpty();
        RuleFor(cart => cart.UserId).NotEmpty();
        RuleFor(cart => cart.Date).NotEmpty();
        RuleFor(cart => cart.Products).NotEmpty();
        RuleFor(cart => cart.Branch)
            .NotEmpty()
            .MinimumLength(2).WithMessage("Branch must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Branch cannot be longer than 100 characters.");
    }
}