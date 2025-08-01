using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Execution;
using System;
using System.Linq;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Extensions
{
  /// <summary>
  /// Extensões do FluentAssertions de validação para coleções.
  /// </summary>
  public static class GenericCollectionAssertionsExtension
  {
    /// <summary>
    /// Validar se contêm combinação pelo filtro.
    /// </summary>
    /// <typeparam name="T">Tipo a ser filtrado.</typeparam>
    /// <param name="assertions">Validador do fluent assertions.</param>
    /// <param name="predicate">Filtro para encontrar elementos.</param>
    /// <param name="because">Motivo da falha.</param>
    /// <param name="becauseArgs">Argumentos da mensagem de falha.</param>
    /// <returns>Continuação do FluentAssertions.</returns>
    public static AndConstraint<GenericCollectionAssertions<T>> ContainMatching<T>(
      this GenericCollectionAssertions<T> assertions,
      Func<T, bool> predicate,
      string because = null,
      params object[] becauseArgs)
    {
      Execute.Assertion
          .BecauseOf(because, becauseArgs)
          .ForCondition(assertions.Subject.Any(predicate))
          .FailWith("Expected collection to contain at least one item matching the predicate, but it does not.");
      return new AndConstraint<GenericCollectionAssertions<T>>(assertions);
    }

    /// <summary>
    /// Validar se não contêm combinação pelo filtro.
    /// </summary>
    /// <typeparam name="T">Tipo a ser filtrado.</typeparam>
    /// <param name="assertions">Validador do fluent assertions.</param>
    /// <param name="predicate">Filtro para encontrar elementos.</param>
    /// <param name="because">Motivo da falha.</param>
    /// <param name="becauseArgs">Argumentos da mensagem de falha.</param>
    /// <returns>Continuação do FluentAssertions.</returns>
    public static AndConstraint<GenericCollectionAssertions<T>> NotContainMatching<T>(this GenericCollectionAssertions<T> assertions, Func<T, bool> predicate, string because = null, params object[] becauseArgs)
    {
      Execute.Assertion
          .BecauseOf(because, becauseArgs)
          .ForCondition(!assertions.Subject.Any(predicate))
          .FailWith("Expected collection not to contain at least one item matching the predicate, but it does.");
      return new AndConstraint<GenericCollectionAssertions<T>>(assertions);
    }
  }
}
