using Ambev.DeveloperEvaluation.WebApi;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.Integration.Application;

/// <summary>
/// Utilitário para testes nos endpoints.
/// </summary>
public class WebApiTestBase : IDisposable
{
    /// <summary>
    /// Cliente http para acesso à api do projeto a ser testada.
    /// </summary>
    public HttpClient HttpClient => Factory.HttpClient;

    /// <summary>
    /// Serviços contidos na DI do projeto destino para serem subsituídos caso necessário no teste.
    /// </summary>
    public Action<IServiceCollection> ConfigureServices { set => Factory.ConfigureServices = value; }

    /// <summary>
    /// Semeador de dados para múltiplos ORMs.
    /// </summary>
    public Action<WebSeeder> Seeder { set => Factory.Seeder = value; }

    /// <summary>
    /// Obter o provedor se serviços.
    /// </summary>
    public IServiceProvider Services => Factory.Services;

    /// <summary>
    /// Fábrica de aplicação destino, ver mais em <see cref="WebApplicationFactory{Program}"/>
    /// </summary>
    internal WebApiFactory<Program> Factory { get; } = new();

    /// <summary>
    /// Libera recursos de teste.
    /// </summary>
    public void Dispose() => Factory.Dispose();
}
