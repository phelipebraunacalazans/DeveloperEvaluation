using Ambev.DeveloperEvaluation.Domain.Events;
using Rebus.Handlers;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.WebApi.Consumers;

public class SaleCreatedConsumer : IHandleMessages<SaleCreatedEvent>
{
    private readonly ILogger _logger;

    public SaleCreatedConsumer(ILogger<SaleCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Handle(SaleCreatedEvent message)
    {
        _logger.LogInformation($"Consuming SaleCreatedEvent: {JsonSerializer.Serialize(message)}");
        return Task.CompletedTask;
    }
}
