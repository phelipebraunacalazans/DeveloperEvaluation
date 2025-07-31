using System;
using System.Collections.Generic;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.Sql
{
  public class SqlQueryResult<TQuery>(
    Func<ParameterCollection, bool> condition,
    IEnumerable<TQuery> data)
    where TQuery : class
  {
    public Func<ParameterCollection, bool> Condition => condition;

    public IEnumerable<TQuery> Data => data;
  }
}
