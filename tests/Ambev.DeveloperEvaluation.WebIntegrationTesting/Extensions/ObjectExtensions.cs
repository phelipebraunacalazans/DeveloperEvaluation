using System;
using System.Collections.Generic;
using System.Linq;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Extensions
{
  public static class ObjectExtensions
  {
    public static Dictionary<string, object> ToDictionary(this object content)
    {
      ArgumentNullException.ThrowIfNull(content, nameof(content));

      return content
        .GetType()
        .GetProperties()
        .ToDictionary(x => x.Name, x => x.GetValue(content, null));
    }
  }
}
