using Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework.Seeding;
using Ambev.DeveloperEvaluation.WebIntegrationTesting.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework
{
  /// <summary>
  /// Represents builder to configure DbContext in memory mocking virtual DbSet properties.
  /// </summary>
  /// <param name="services">A delegate for configuring the <see cref="IServiceCollection"/>.</param>
  public sealed class InMemoryDbContextServiceBuilder<T>(IServiceCollection services)
    where T : DbContext
  {
    private readonly IServiceCollection _services = services ?? throw new ArgumentNullException(nameof(services));

    /// <summary>
    /// Adding seeder to create data into mocked DbSet in memory.
    /// </summary>
    /// <typeparam name="TSeeder">Implementation of <see cref="ISeedable"/>.</typeparam>
    /// <param name="seederActivator">To create new instance.</param>
    /// <param name="seederAcesssor">To retrieve created instance.</param>
    /// <returns><see cref="InMemoryDbContextServiceBuilder{T}"/></returns>
    public InMemoryDbContextServiceBuilder<T> WithSeeder<TSeeder>(
      Func<IServiceProvider, Mock<T>> mockDbContextActivator,
      Func<IDataContext, TSeeder> seederActivator,
      Func<Action<TSeeder>> seederAcesssor)
      where TSeeder : ISeedable
    {
      ArgumentNullException.ThrowIfNull(mockDbContextActivator, nameof(mockDbContextActivator));
      ArgumentNullException.ThrowIfNull(seederActivator, nameof(seederActivator));
      ArgumentNullException.ThrowIfNull(seederAcesssor, nameof(seederAcesssor));

      _services.RemoveAllServices<T>();

      _services.AddSingleton(ctx =>
        SimpleContext.Create(
          () => mockDbContextActivator(ctx),
          seederActivator,
          seederAcesssor()));

      return this;
    }
  }
}
