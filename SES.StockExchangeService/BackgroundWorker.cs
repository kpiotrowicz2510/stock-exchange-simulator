using System.Text.Json;
using MediatR;
using SES.Shared.Contract;
using SES.Shared.Extensions;
using SES.Shared.Infrastructure;
using StockExchangeService.Commands.CreateStockBid;
using StockExchangeService.Commands.RemoveStockBid;
using StockExchangeService.Queries.GetOpenBidsForStock;
using BidType = StockExchangeService.Queries.Entities.BidType;
using StockBid = StockExchangeService.Queries.Entities.StockBid;

namespace StockExchangeService;

public sealed class BackgroundWorker : BackgroundService
{
    private readonly ILogger<BackgroundWorker> _logger;
    private readonly IMqClient _client;
    private readonly IMediator _mediator;
    
    //Future DB context
    private User _user = new User(Ulid.NewUlid(), "User", "123", "test@test.pl", 0.0);

    public BackgroundWorker(ILogger<BackgroundWorker> logger, IMqClient client, IMediator mediator)
    {
        _logger = logger;
        _client = client;
        _mediator = mediator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _client.RegisterCallback("ses-exchange", (message) => HandleMessage(message));
        
        while (!stoppingToken.IsCancellationRequested)
        {
            
        }
    }

    private async Task CheckForBuySellTransactions(Commands.Entities.StockBid stockBid)
    {
        var availableBuyBids = await _mediator.Send<List<StockBid>>(
            new GetOpenBidsForStockCommand(stockBid.CompanyStockId, BidType.Buy, stockBid.PricePerShare, stockBid.Amount)
        );

        foreach (var buyBid in availableBuyBids)
        {
            var availableSellBids = await _mediator.Send<List<StockBid>>(
                new GetOpenBidsForStockCommand(buyBid.CompanyStockId, BidType.Sell, buyBid.PricePerShare, buyBid.Amount)
            );

            foreach (var sellBid in availableSellBids)
            {
                PublishTransaction(buyBid, sellBid);
            }
        }
    }

    private async Task PublishTransaction(StockBid buyBid, StockBid sellBid)
    {
        var transactionForSeller = new StockTransaction(
            Ulid.NewUlid(),
            sellBid.ProcessingId,
            sellBid.CompanyStockId,
            sellBid.PricePerShare,
            sellBid.Amount,
            DateTime.UtcNow,
            sellBid.Id,
            buyBid.UserId
        );
    
        _client.Publish(
            "",
            "ses-transaction",
            JsonSerializer.Serialize(transactionForSeller),
            null);

        await _mediator.Publish(new RemoveStockBidCommand(buyBid.Id));
        await _mediator.Publish(new RemoveStockBidCommand(sellBid.Id));
    }

    private async Task HandleMessage(string message)
    {
        var stockBid = JsonSerializer.Deserialize<Commands.Entities.StockBid>(message);
    
        Console.WriteLine($" [x] Received {stockBid.BidType} for {stockBid.CompanyStockId}");
    
        await _mediator.Send(new CreateStockBidCommand(
            stockBid.Id,
            stockBid.ProcessingId,
            stockBid.CompanyStockId,
            stockBid.PricePerShare,
            stockBid.Amount,
            stockBid.CreationDateTime,
            stockBid.UserId,
            stockBid.BidType));

        Console.WriteLine($" [x] Processing {stockBid.BidType} for {stockBid.CompanyStockId}");
        
        await CheckForBuySellTransactions(stockBid);
        
        Console.WriteLine($" [x] Finished {stockBid.BidType} for {stockBid.CompanyStockId}");
    }
}