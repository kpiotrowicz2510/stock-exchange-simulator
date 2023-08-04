using System.Text.Json;
using SES.Shared.Contract;
using SES.Shared.Extensions;
using SES.Shared.Infrastructure;

namespace PaymentSystemService;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMqClient _client;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Ulid _userId = Ulid.NewUlid();

    public Worker(ILogger<Worker> logger, IMqClient client, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _client = client;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _client.RegisterCallback("ses-payment",  (message) => HandleMessage(message));
        
        while (!stoppingToken.IsCancellationRequested)
        {
            Thread.Sleep(5000);
        }
    }

    private async Task HandleMessage(string message)
    {
        using var client = _httpClientFactory.CreateClient();
        var transaction = JsonSerializer.Deserialize<TopUpTransaction>(message);
        //var result = await client.GetStringAsync($"payment/{transaction.Id}");
        
        Thread.Sleep(50);

        var result = transaction.Id.ToString();
        
        _logger.LogInformation(result);
        
        if (result.Equals(transaction.Id.ToString()))
        {
            transaction = transaction with
            {
                Status = new TopUpTransactionStatus(Ulid.NewUlid(), "Ready", "Mock", true)
            };
            _client.Publish(
                "", 
                "ses-usermanagement", 
                JsonSerializer.Serialize(transaction),
                null);
        }
    }
}