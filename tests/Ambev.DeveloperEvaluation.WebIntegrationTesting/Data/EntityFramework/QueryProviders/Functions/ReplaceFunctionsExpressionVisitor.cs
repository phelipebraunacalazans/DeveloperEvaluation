using Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework.QueryProviders.Functions.Mocks;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework.QueryProviders.Functions
{
  internal class ReplaceFunctionsExpressionVisitor : ExpressionVisitor
  {
    private readonly Dictionary<MethodInfo, MethodInfo> _mocks = new()
    {
      [ILikeMock.Original] = ILikeMock.Replacement,
      [UnaccentMock.Original] = UnaccentMock.Replacement,
    };

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
      if (_mocks.TryGetValue(node.Method, out var mock))
      {
        node = Expression.Call(mock, node.Arguments);
      }

      return base.VisitMethodCall(node);
    }
  }
}
