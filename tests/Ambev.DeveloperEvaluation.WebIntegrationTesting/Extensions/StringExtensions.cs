using System.Text.RegularExpressions;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Extensions
{
  public static class StringExtensions
  {
    private static readonly Regex RegexJsonPattern = new Regex("(?<json>{(?:[^{}]|(?<Nested>{)|(?<-Nested>}))*(?(Nested)(?!))})", RegexOptions.Compiled);

    public static bool IsValidJson(this string content)
    {
      return RegexJsonPattern.IsMatch(content);
    }
  }
}
