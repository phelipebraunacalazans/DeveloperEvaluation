using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Services;

public class SaleDiscountService
{
    private static readonly List<(int MinQuantity, int MaxQuantity, int Percentage)> Discounts =
    [
        (4, 9, 10),
        (10, 20, 20),
    ];

    public void ApplyDiscounts(Cart cart)
    {
        ArgumentNullException.ThrowIfNull(cart, nameof(cart));

        foreach (var item in cart.Items)
        {
            var discount = Discounts.FirstOrDefault(d => item.Quantity >= d.MinQuantity && item.Quantity <= d.MaxQuantity);

            item.ApplyDiscount(discount.Percentage);
        }

        cart.RefreshTotalAmount();
    }
}
