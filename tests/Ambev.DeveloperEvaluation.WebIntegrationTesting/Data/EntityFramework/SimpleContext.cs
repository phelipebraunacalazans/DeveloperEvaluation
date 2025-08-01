using Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework.Moq;
using Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework.Seeding;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework
{
  public static class SimpleContext
  {
    public static TDbContext Create<TDbContext, TSeeder>(
      Func<Mock<TDbContext>> mockDbContextActivator,
      Func<IDataContext, TSeeder> seederActivator,
      Action<TSeeder>? method = null)
      where TDbContext : DbContext
      where TSeeder : ISeedable
    {
      ArgumentNullException.ThrowIfNull(mockDbContextActivator, nameof(mockDbContextActivator));
      ArgumentNullException.ThrowIfNull(seederActivator, nameof(seederActivator));

      var instances = new MemoryContainer();
      var seeder = seederActivator(instances);
      seeder.InitializeDefaultEntities();

      method?.Invoke(seeder);

      var mockDbContext = mockDbContextActivator();

      var contextBuilder = new MockDbContextBuilder<TDbContext>(mockDbContext)
        .EmptyAll();

      contextBuilder.AddValues(instances.GetItems());

      // contextBuilder.AddQueryResults(instances.GetQueryResults());

      contextBuilder.AddEntityIdGenerator(true);

      // contextBuilder.AddTransaction();

      contextBuilder.AddDatabase();

      // contextBuilder.FakeChangeTracker();

      return contextBuilder.Build();
    }
  }
}
