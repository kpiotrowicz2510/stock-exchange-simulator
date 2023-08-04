using System.Text.Json;
using SES.Shared.Contract;
using SES.Shared.Extensions;
using SES.Shared.Infrastructure;

Ulid SystemUserId = Ulid.NewUlid();

Action<string> loggerCallback = (message) =>
{
    var stockBid = JsonSerializer.Deserialize<StockBid>(message);

    Console.WriteLine($" [x] Received {stockBid}");
};

var mqClient = MqClientFactory
    .CreateMqClient(MqClients.RabbitMQ, "ses-rabbitmq");

Action<string> messageCallback = (message) =>
{
    var stockBid = JsonSerializer.Deserialize<StockBid>(message);

    Console.WriteLine($" [x] Received {stockBid.BidType} for {stockBid.CompanyStockId}");
    Console.WriteLine($" [x] Processing {stockBid.BidType} for {stockBid.CompanyStockId}");
    Thread.Sleep(500);

    var transaction = new StockTransaction(
        Ulid.NewUlid(),
        stockBid.ProcessingId,
        stockBid.CompanyStockId,
        stockBid.PricePerShare,
        stockBid.Amount,
        DateTime.Now,
        SystemUserId,
        stockBid.UserId
    );
    
    mqClient.Publish(
        "",
        "ses-transaction",
        JsonSerializer.Serialize(transaction),
        null);
    
    Console.WriteLine($" [x] Finished {stockBid.BidType} for {stockBid.CompanyStockId}");
};

mqClient.RegisterCallback("ses-exchange", messageCallback);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(mqClient);

var app = builder.Build();

app.Run();