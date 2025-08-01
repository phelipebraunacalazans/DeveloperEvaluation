using System.Text.Json;
using Ambev.DeveloperEvaluation.Domain.Events;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.WebApi.Consumers;

public class SaleModifiedConsumer : IHandleMessages<SaleModifiedEvent>
{
    private readonly ILogger _logger;

    public SaleModifiedConsumer(ILogger<SaleModifiedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleModifiedEvent message)
    {
        _logger.LogInformation($"Consuming SaleModifiedEvent: {JsonSerializer.Serialize(message)}");
        return Task.CompletedTask;
    }
}