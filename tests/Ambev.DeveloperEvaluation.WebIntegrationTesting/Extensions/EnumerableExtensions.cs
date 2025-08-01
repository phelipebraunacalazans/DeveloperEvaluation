using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Extensions
{
  public static class EnumerableExtensions
  {
    /// <summary>
    /// Extension for <see cref="IEnumerable{T}"/> object to handle each item.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="sequence">The enumerable sequence.</param>
    /// <param name="action">Action to apply to each item.</param>
    public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
    {
      ArgumentNullException.ThrowIfNull(sequence, nameof(sequence));

      foreach (var item in sequence)
      {
        action(item);
      }
    }
  }
}
