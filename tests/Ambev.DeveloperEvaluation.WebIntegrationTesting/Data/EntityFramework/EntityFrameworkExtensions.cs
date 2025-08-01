using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework
{
  public static class EntityFrameworkExtensions
  {
    public static InMemoryDbContextServiceBuilder<T> UseInMemoryDbContext<T>(this IServiceCollection services)
        where T : DbContext
    {
      ArgumentNullException.ThrowIfNull(services, nameof(services));

      var dbContextDescriptor = services.SingleOrDefault(
          d => d.ServiceType ==
              typeof(DbContextOptions<T>));

      services.Remove(dbContextDescriptor);

      return new InMemoryDbContextServiceBuilder<T>(services);
    }
  }
}
