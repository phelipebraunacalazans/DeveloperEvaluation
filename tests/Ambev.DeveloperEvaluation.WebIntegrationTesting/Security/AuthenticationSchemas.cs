namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Security
{
  /// <summary>
  ///   Define constantes schemas de autenticação.
  /// </summary>
  public static class AuthenticationSchemas
  {
    /// <summary>
    ///   Schema basic <seealso href="https://datatracker.ietf.org/doc/html/rfc7617" />.
    /// </summary>
    public static readonly string Basic = "Basic";

    /// <summary>
    /// Bearer token <seealso href="https://datatracker.ietf.org/doc/html/rfc6750" />.
    /// </summary>
    public static readonly string Bearer = "Bearer";
  }
}
