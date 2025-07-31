using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart;

public class GetCartValidator : AbstractValidator<GetCartCommand>
{
    /// <summary>
    /// Initializes validation rules for <see cref="GetCartCommand"/>.
    /// </summary>
    public GetCartValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Cart ID is required");
    }
}
