using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

public class GetCartRequestValidator : AbstractValidator<GetCartRequest>
{
    /// <summary>
    /// Initializes validation rules for <see cref="GetCartRequest"/>.
    /// </summary>
    public GetCartRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Cart ID is required");
    }
}