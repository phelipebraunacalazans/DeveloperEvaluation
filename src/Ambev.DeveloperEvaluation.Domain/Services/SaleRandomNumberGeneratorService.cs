namespace Ambev.DeveloperEvaluation.Domain.Services;

public class SaleRandomNumberGeneratorService
{
    public long GenerateNext()
    {
        var number = DateTime.UtcNow.ToString("ddMMyyyyhhmmssf") + Random.Shared.NextInt64(1, 999).ToString();
        return long.Parse(number);
    }
}