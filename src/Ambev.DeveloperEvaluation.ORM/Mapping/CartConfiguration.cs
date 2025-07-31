using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM.ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(x => x.SaleNumber).IsRequired();
        builder.Property(x => x.TotalSaleAmount).IsRequired();
        builder.Property(x => x.StoreName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.PurchaseStatus).IsRequired();

        builder.Property(x => x.SoldAt).IsRequired().HasColumnName("SaleAt");
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired(false);
        builder.Property(x => x.CancelledAt).IsRequired(false);

        builder.HasOneAsShadow(x => x.CreatedBy);
        builder.HasOneAsShadow(x => x.BoughtBy);
        builder.HasOneAsShadow(x => x.CancelledBy, required: false);

        builder.HasMany(x => x.Items)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.SaleNumber)
            .IsUnique();
    }
}