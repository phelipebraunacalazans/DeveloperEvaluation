using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleModifiedEvent
{
    public required Guid CartId { get; init; }
    public required Guid CustomerId { get; init; }
    public required string CustomerName { get; init; }
    public required int TotalProducts { get; init; }
    public required decimal TotalAmount { get; init; }
    public required ICollection<SaleProduct> Products { get; init; }

    public static SaleModifiedEvent CreateFrom(Cart cart)
    {
        return new SaleModifiedEvent
        {
            CartId = cart.Id,
            CustomerId = cart.BoughtById,
            CustomerName = cart.BoughtBy.Username,
            TotalProducts = cart.Items.Count,
            TotalAmount = cart.TotalSaleAmount,
            Products = cart.Items.Select(i => new SaleProduct
            {
                ProductId = i.ProductId,
                Title = i.Product.Name,
                Price = i.Product.Price,
                DiscountAmount = i.DiscountAmount,
                DiscountPercent = i.DiscountPercentage,
                TotalAmount = i.TotalAmount,
            }).ToArray()
        };
    }

    public record SaleProduct
    {
        public required Guid ProductId { get; init; }
        public required string Title { get; init; }
        public required decimal Price { get; init; }
        public required decimal DiscountPercent { get; init; }
        public required decimal DiscountAmount { get; init; }
        public required decimal TotalAmount { get; init; }
    };
}
