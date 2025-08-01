using Ambev.DeveloperEvaluation.WebIntegrationTesting.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework.TrackChanges
{
    public class DbTrackEntity<T>(List<PropertyChange> properties)
    where T : class
  {
    private readonly List<PropertyChange> _properties = properties ?? throw new ArgumentNullException(nameof(properties));

    public DbTrackEntity<T> Property<TValue>(
      Expression<Func<T, TValue>> expression,
      TValue originalValue = default,
      TValue modifiedValue = default,
      PropertyChangeStateType state = PropertyChangeStateType.Modified)
    {
      var propertyInfo = expression.GetPropertyFromExpression();

      _properties.Add(new()
      {
        PropertyInfo = propertyInfo,
        PropertyName = propertyInfo.Name,
        PropertyType = propertyInfo.PropertyType,
        ChangeState = state,
        OriginalValue = originalValue,
        CurrentValue = modifiedValue,
      });

      return this;
    }
  }
}
