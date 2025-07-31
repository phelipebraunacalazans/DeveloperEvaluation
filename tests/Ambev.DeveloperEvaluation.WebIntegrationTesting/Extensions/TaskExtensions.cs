using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Extensions
{
  public static class TaskExtensions
  {
    public static Task<T> AsTask<T>(this T instance) => Task.FromResult(instance);
  }
}
