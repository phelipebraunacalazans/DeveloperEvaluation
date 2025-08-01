using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework;
using Ambev.DeveloperEvaluation.WebIntegrationTesting.Data.EntityFramework.TrackChanges;
using Ambev.DeveloperEvaluation.WebIntegrationTesting.Extensions;
using Ambev.DeveloperEvaluation.WebIntegrationTesting.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;

namespace Ambev.DeveloperEvaluation.Integration.Application;

/// <summary>
/// Fábrica para criar novas instâncias da aplicação destino, a que irá ser testada.
/// </summary>
/// <typeparam name="TProgram">Tipo da classe Program.cs da api destino.</typeparam>
public class WebApiFactory<TProgram> : WebApplicationFactory<TProgram>
  where TProgram : class
{
    private readonly Dictionary<Type, Mock> _mocks = [];
    private HttpClient? _httpClient;

    /// <summary>
    /// Cliente http para acesso à api do projeto a ser testada.
    /// </summary>
    public HttpClient HttpClient => _httpClient ??= CreateClient();

    /// <summary>
    /// Semeador de dados para múltiplos ORMs.
    /// </summary>
    public Action<WebSeeder>? Seeder { get; set; }

    /// <summary>
    /// Injeta claims do usuário da requisição http.
    /// </summary>
    public UserClaims User { get; init; } = new()
    {
        Id = Guid.NewGuid(),
        Username = "Fernando",
        Email = "fernando@gmail.com",
        Role = "Customer",
    };

    /// <summary>
    /// Serviços contidos na DI do projeto destino para serem subsituídos caso necessário no teste.
    /// </summary>
    public Action<IServiceCollection>? ConfigureServices { get; set; }

    /// <summary>
    /// Informar propriedades das entidades que foram alteradas no EF para uso do método ChangeTracker.Entries.
    /// </summary>
    public Action<DbTrackEntities>? DbTrackChanges { get; set; }

    /// <summary>
    /// Obtém mock do tipo especificado já criado e usado no programa.
    /// </summary>
    /// <typeparam name="T">Tipo mockado</typeparam>
    /// <returns>Mock do tipo especificado.</returns>
    /// <exception cref="Exception">Caso o mock tiver sido criado.</exception>
    public Mock<T> GetMock<T>()
      where T : class
    {
        var type = typeof(T);
        return _mocks.TryGetValue(type, out var value) && value is Mock<T> mock
          ? mock
          : throw new Exception($"There is no mock registered for type \"{type.Name}\"!");
    }

    /// <summary>
    /// Indica se há instância de mock registrado no contexto da aplicação.
    /// </summary>
    /// <typeparam name="T">Tipo da instância do mock</typeparam>
    /// <returns>True caso o mock tenha uma instância.</returns>
    public bool HasMock<T>()
      where T : class =>
      _mocks.ContainsKey(typeof(T));

    /// <summary>
    /// Obtém instância do Mock do tipo especificado.
    /// </summary>
    /// <typeparam name="T">Tipo mockado.</typeparam>
    /// <returns>Instância do Mock pelo tipo escificado.</returns>
    /// <exception cref="Exception">Caso o mock tiver sido criado.</exception>
    public T GetMockedObject<T>()
      where T : class
    {
        return GetMock<T>().Object;
    }

    /// <summary>
    /// Limpar cache baseado no MemoryCache do dotnet.
    /// </summary>
    public void ClearCache()
    {
        var cache = Services.GetService<IMemoryCache>() as MemoryCache;
        cache?.Compact(1);
    }

    /// <inheritdoc/>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        // note: disable reload on change file watcher configuration to not occur timeout in testserver
        builder.ConfigureAppConfiguration((hostingContext, configBuilder) =>
          configBuilder.Sources.Where(s => s is FileConfigurationSource)
            .ForEach(s => ((FileConfigurationSource)s).ReloadOnChange = false));

        builder.UseSetting("UseSwaggerUI", bool.FalseString);

        builder.ConfigureServices(services =>
        {
            services.UseInMemoryDbContext<DefaultContext>()
              .WithSeeder(
                NewMockMesaDbContext,
                ctx => new WebSeeder(ctx),
                () => Seeder ?? (seeder => { }));

            ConfigureServices?.Invoke(services);
        });

        builder.UseEnvironment("Development");
    }

    /// <inheritdoc/>
    protected override void ConfigureClient(HttpClient client)
    {
        base.ConfigureClient(client);

        client.DefaultRequestHeaders.Add("Authorization", $"{AuthenticationSchemas.Bearer} {GenerateAuthorizationBearerToken()}");

        client.Timeout = TimeSpan.FromMinutes(3);
    }

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
                _httpClient = null;
            }
        }
    }

    /// <summary>
    /// Cria nova instância mockada de <see cref="MesaDbContext"/>.
    /// </summary>
    /// <param name="services">Provedor de serviços</param>
    /// <returns>Mock de <see cref="MesaDbContext"/></returns>
    private Mock<DefaultContext> NewMockMesaDbContext(IServiceProvider services)
    {
        var mockMesaDbContext = RegisterMock<DefaultContext>(new(
          new DbContextOptions<DefaultContext>()));

        ICollection<EntityEntry> trackerEntriesResult = []; // note: precisa sempre retornar alguma coisa porque sempre é consultado o contexto do EF para saber se houve alteração para registrar no log. Ocorre exception se não mockar resultado, mesmo que vazio.

        if (DbTrackChanges != null)
        {
            Dictionary<Type, EntityChange> types = [];
            DbTrackChanges(new DbTrackEntities(types));
            trackerEntriesResult = EntityEntryMocker.MockFor(types);
        }

        //mockMesaDbContext.Setup(_ => _.GetChangeTrackerEntries())
        //   .Returns(trackerEntriesResult);

        return mockMesaDbContext;
    }

    /// <summary>
    /// Registra mock para consulta posterior.
    /// </summary>
    /// <typeparam name="T">Tipo do mock.</typeparam>
    /// <param name="mock">Instância do mock.</param>
    /// <returns>Mock do tipo.</returns>
    private Mock<T> RegisterMock<T>(Mock<T> mock)
      where T : class
    {
        var key = typeof(T);
        return (_mocks.TryGetValue(key, out Mock? value) ? value : _mocks[key] = mock) as Mock<T> ?? mock;
    }

    private string GenerateAuthorizationBearerToken()
    {
        IConfiguration configuration = new ConfigurationManager()
          .AddInMemoryCollection(new Dictionary<string, string?>()
          {
            { "Jwt:SecretKey", "YourSuperSecretKeyForJwtTokenGenerationThatShouldBeAtLeast32BytesLong" },
          })
          .Build();
        var jwtTokenGenerator = new JwtTokenGenerator(configuration);
        return jwtTokenGenerator.GenerateToken(User);
    }
}
