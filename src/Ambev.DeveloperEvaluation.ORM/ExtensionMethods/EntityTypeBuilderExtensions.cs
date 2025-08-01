using System.Linq.Expressions;
using Ambev.DeveloperEvaluation.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.ExtensionMethods;

public static class EntityTypeBuilderExtensions
{
    /// <summary>
    /// Mapping foreign key as shadow relationship.
    /// </summary>
    /// <typeparam name="TEntity">Type of principal entity.</typeparam>
    /// <typeparam name="TRelatedEntity">Type of related entity.</typeparam>
    /// <param name="builder">Entity type builder</param>
    /// <param name="navigation">Expression with the property navigation</param>
    /// <param name="deleteBehavior">Behavior when delete</param>
    /// <exception cref="ArgumentException">Occurs when a expression is not of type <see cref="MemberExpression"/></exception>
    public static void HasOneAsShadow<TEntity, TRelatedEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TRelatedEntity?>> navigation,
        bool required = true,
        DeleteBehavior deleteBehavior = DeleteBehavior.NoAction)
        where TEntity : class
        where TRelatedEntity : class
    {
        ArgumentNullException.ThrowIfNull(builder, nameof(builder));
        ArgumentNullException.ThrowIfNull(navigation, nameof(navigation));

        if (navigation.Body is not MemberExpression expression)
        {
            throw new ArgumentException($"Expression '{navigation.ToString()}' refers to a method, not a property.");
        }

        string propertyName = expression.Member.Name;

        var propertyType = required ? typeof(Guid) : typeof(Guid?);
        builder.Property(propertyType, propertyName + nameof(BaseEntity.Id))
            .IsRequired(required);

        builder.HasOne(navigation)
            .WithMany()
            .HasForeignKey(propertyName + nameof(BaseEntity.Id))
            .OnDelete(deleteBehavior)
            .IsRequired(required);
    }
}