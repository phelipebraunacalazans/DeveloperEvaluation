using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Specifications;

public class SaleLimitReachedSpecification : ISpecification<Cart>
{
    private readonly int _maximumItemsPerProduct = 20;

    public bool IsSatisfiedBy(Cart cart)
    {
        return cart.Items
            .Any(i => i.Quantity > _maximumItemsPerProduct);
    }
}