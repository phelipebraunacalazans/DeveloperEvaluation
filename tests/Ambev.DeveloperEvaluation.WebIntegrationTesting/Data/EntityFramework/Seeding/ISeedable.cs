namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework.Seeding
{
  /// <summary>
  /// Defines way to create data.
  /// </summary>
  public interface ISeedable
  {
    /// <summary>
    /// Create default entities before to seed.
    /// </summary>
    void InitializeDefaultEntities()
    {
    }
  }
}
