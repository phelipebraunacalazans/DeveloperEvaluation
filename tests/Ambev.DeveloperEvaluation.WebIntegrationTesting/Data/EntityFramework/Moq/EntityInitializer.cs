using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.WebIntegrationTesting.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework.Moq
{
    internal class EntityInitializer
  {
    public static IEnumerable<BaseEntity> Run(IEnumerable<BaseEntity> entities)
    {
      var visited = entities
        .Select(e => e.WithRandomIdIfEmpty())
        .ToHashSet();
      var current = new Stack<BaseEntity>(visited);
      var newEntities = new List<BaseEntity>();

      while (current.TryPop(out var entity))
      {
        var properties = entity.GetType()
          .GetProperties(BindingFlags.Instance | BindingFlags.Public)
          .Where(p => !p.PropertyType.IsPrimitive && p.PropertyType != typeof(string))
          .Where(p => p.PropertyType.IsAssignableTo(typeof(BaseEntity)) ||
                      p.PropertyType.IsAssignableTo(typeof(IEnumerable<BaseEntity>)));

        foreach (var property in properties)
        {
          var value = property.GetValue(entity);

          if (value is IEnumerable<BaseEntity> newRelatedEntities)
          {
            newRelatedEntities
              .Where(e => e.Id == Guid.Empty && visited.Add(e.WithRandomIdIfEmpty()))
              .ForEach(e =>
              {
                newEntities.Add(e);
                current.Push(e);
              });
          }
          else if (value is BaseEntity relatedEntity && relatedEntity.Id == Guid.Empty && visited.Add(relatedEntity.WithRandomIdIfEmpty()))
          {
            newEntities.Add(relatedEntity);
            current.Push(relatedEntity);
          }
        }
      }

      return newEntities;
    }
  }
}
