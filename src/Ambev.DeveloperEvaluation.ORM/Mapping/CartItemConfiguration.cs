using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class CartItemConfiguration : IEntityTypeConfiguration<CartItem>
{
    public void Configure(EntityTypeBuilder<CartItem> builder)
    {
        builder.ToTable("CartItems");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.Quantity).IsRequired();
        builder.Property(x => x.UnitPrice).IsRequired().HasPrecision(10, 2);
        builder.Property(x => x.DiscountPercentage).IsRequired().HasPrecision(5, 2);
        builder.Property(x => x.DiscountAmount).IsRequired().HasPrecision(10, 2);
        builder.Property(x => x.TotalPreDiscounts).IsRequired().HasPrecision(14, 2);
        builder.Property(x => x.TotalAmount).IsRequired().HasPrecision(14, 2);
        builder.Property(x => x.PurchaseStatus).IsRequired();

        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired(false);
        builder.Property(x => x.CancelledAt).IsRequired(false);

        builder.HasOneAsShadow(x => x.CreatedBy);
        builder.HasOneAsShadow(x => x.CancelledBy, required: false);

        builder.HasOneAsShadow(x => x.Product, deleteBehavior: DeleteBehavior.SetNull);
    }
}
