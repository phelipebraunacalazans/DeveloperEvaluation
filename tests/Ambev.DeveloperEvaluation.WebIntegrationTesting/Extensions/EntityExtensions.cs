using Ambev.DeveloperEvaluation.Domain.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using static System.Text.Json.JsonNamingPolicy;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Extensions
{
    public static class EntityExtensions
  {
    private static readonly MethodInfo SetId = typeof(BaseEntity)
        .GetProperty(nameof(BaseEntity.Id))?.GetSetMethod(true)
            ?? throw new MissingMemberException($"Cannot get method {nameof(BaseEntity.Id)} from {nameof(BaseEntity)}.");

    public static TEntity WithRandomId<TEntity>(this TEntity entity)
        where TEntity : BaseEntity
    {
      object entityValueOfKey;

      entityValueOfKey = Guid.Empty;

      return entity.WithId(entityValueOfKey);
    }

    public static TEntity WithRandomIdIfEmpty<TEntity>(this TEntity entity)
        where TEntity : BaseEntity
    {
      return entity.Id == Guid.Empty
          ? entity.WithRandomId()
          : entity;
    }

    public static TEntity WithId<TEntity>(this TEntity entity, object id)
        where TEntity : BaseEntity
    {
      SetId.Invoke(entity, [id]);
      return entity;
    }

    public static TEntity SetProperty<TEntity, TValue>(this TEntity entity, Expression<Func<TEntity, TValue>> expression, TValue value)
    {
      var propertyInfo = GetPropertyFromExpression(expression);
      propertyInfo.SetValue(entity, value);
      return entity;
    }

    public static TEntity SetAsNull<TEntity, TValue>(this TEntity entity, Expression<Func<TEntity, TValue>> expression)
    {
      var propertyInfo = GetPropertyFromExpression(expression);
      propertyInfo.SetValue(entity, null);
      return entity;
    }

    public static TEntity AddToReadOnlyColletion<TEntity, TItem>(this TEntity entity, Expression<Func<TEntity, IReadOnlyCollection<TItem>>> expression, TItem item)
        where TEntity : BaseEntity
    {
      var collection = expression.Compile()
          .Invoke(entity);

      if (collection is ICollection<TItem> modifiableCollection && !modifiableCollection.IsReadOnly)
      {
        modifiableCollection.Add(item);
        return entity;
      }

      var propertyInfo = GetPropertyFromExpression(expression);
      var type = typeof(TEntity);

      var nameInCamelCase = CamelCase.ConvertName(propertyInfo.Name);
      var nameWithUnderscore = $"_{nameInCamelCase}";
      var flags = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetField;

      var backingField = type.GetField(propertyInfo.Name, flags)
          ?? type.GetField(nameWithUnderscore, flags)
          ?? throw new InvalidOperationException($"Could not find a field named '{nameInCamelCase}' or '{nameWithUnderscore}'.");

      var backingColletion = backingField.GetValue(entity);

      if (backingColletion is ICollection<TItem> modifiableBackingCollection && !modifiableBackingCollection.IsReadOnly)
      {
        modifiableBackingCollection.Add(item);
        return entity;
      }

      throw new InvalidOperationException($"'{backingField.Name}' is not a modifiabled colletion.");
    }

    public static IEnumerable<BaseEntity> GetAllRelatedEntities(this BaseEntity entity)
    {
      var test = new List<BaseEntity>();
      if (entity.Id == Guid.Empty)
      {
        test.Add(entity);
      }

      var visited = new HashSet<BaseEntity> { entity.WithRandomIdIfEmpty() };
      var current = new Stack<BaseEntity>(visited);

      while (current.TryPop(out var currentEntity))
      {
        var properties = currentEntity.GetType()
          .GetProperties(BindingFlags.Instance | BindingFlags.Public)
          .Where(p => p.PropertyType.IsAssignableTo(typeof(BaseEntity)) ||
                      p.PropertyType.IsAssignableTo(typeof(IEnumerable)));

        foreach (var property in properties)
        {
          var value = property.GetValue(currentEntity);

          if (value is BaseEntity newEntity && visited.Add(newEntity.WithRandomIdIfEmpty()))
          {
            test.Add(newEntity);
            current.Push(newEntity);
          }
          else if (value is IEnumerable<BaseEntity> newEntities)
          {
            newEntities
              .Where(e => visited.Add(e.WithRandomIdIfEmpty()))
              .ForEach(e =>
              {
                test.Add(e);
                current.Push(e);
              });
          }
        }
      }

      return test;
    }

    public static PropertyInfo GetPropertyFromExpression<TEntity, TValue>(this Expression<Func<TEntity, TValue>> expression)
    {
      ArgumentNullException.ThrowIfNull(expression, nameof(expression));

      if (expression.Body is MemberExpression member && member.Member is PropertyInfo propertyInfo)
      {
        return propertyInfo;
      }

      throw new ArgumentException($"Expression '{expression}' is not a property.", nameof(expression));
    }
  }
}
