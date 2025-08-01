using System.Collections.Generic;
using System.Linq;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.Sql
{
  internal interface ISqlQueryResultCollection
  {
    IQueryable Execute(object[] parameters);

    IQueryable Execute(IDictionary<string, object> parameters);

    IQueryable<T> Execute<T>(IDictionary<string, object> parameters);
  }
}
