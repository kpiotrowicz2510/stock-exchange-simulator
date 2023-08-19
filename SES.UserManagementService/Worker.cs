using System.Text.Json;
using SES.Shared.Contract;
using SES.Shared.Extensions;
using SES.Shared.Infrastructure;

namespace UserManagementService;

public record PendingDeduction(Ulid Id, double Amount);

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMqClient _client;

    private List<PendingDeduction> PendingDeductions = new();
    
    //Future DB context
    private User _user = new User(Ulid.NewUlid(), "User", "123", "test@test.pl", 0.0);

    public Worker(ILogger<Worker> logger, IMqClient client)
    {
        _logger = logger;
        _client = client;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _client.RegisterCallback("ses-usermanagement", (message) => HandleMessage(message));
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var topUpTransaction = new TopUpTransaction(
                Ulid.NewUlid(),
                _user.Id, 
                "Mock", 
                100.0, 
                DateTime.Now, 
                new TopUpTransactionStatus(Ulid.NewUlid(), "New", "Mock", false));
            
            _client.Publish(
                "", 
                "ses-payment", 
                JsonSerializer.Serialize(topUpTransaction), 
                null);
            
            Thread.Sleep(200);
        }
    }

    private void HandleMessage(string message)
    {
        var transaction = JsonSerializer.Deserialize<TopUpTransaction>(message);

        if (transaction.Status.IsFinal)
        {
            _logger.LogInformation($"Updating user balance {transaction.Id} Balance to Add: {transaction.Amount} Current status: {transaction.Status.Status}");
            _user = _user with { AvailableBalance = _user.AvailableBalance + transaction.Amount };
        }else
        {
            _logger.LogInformation($"Awaiting confirmation for {transaction.Id} Current status: {transaction.Status.Status}");
        }
        _logger.LogInformation($"Current user {_user.Id} balance: {_user.AvailableBalance - PendingDeductions.Sum(x=>x.Amount)} PLN");

        if (_user.AvailableBalance - PendingDeductions.Sum(x=>x.Amount) > 500)
        {
            _logger.LogInformation($"Current user {_user.Id} buying stock");

            var buyRequest = new StockBid(
                Ulid.NewUlid(),
                Ulid.NewUlid(),
                Ulid.NewUlid(),
                10.0,
                50,
                DateTime.UtcNow,
                _user.Id,
                Random.Shared.Next(1,100) > 50 ? BidType.Buy : BidType.Sell
            );
            
            _client.Publish(
                "",
                "ses-exchange",
                JsonSerializer.Serialize(buyRequest),
                null
                );

            PendingDeductions.Add(new PendingDeduction(buyRequest.ProcessingId, buyRequest.PricePerShare * buyRequest.Amount));
        }
    }
}