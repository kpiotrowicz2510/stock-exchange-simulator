using System.Text.Json;
using SES.Shared.Contract;
using SES.Shared.Extensions;
using SES.Shared.Infrastructure;

Ulid SystemUserId = Ulid.NewUlid();

var mqClient = MqClientFactory
    .CreateMqClient(MqClients.RabbitMQ, "ses-rabbitmq");

Action<string> messageCallback = (message) =>
{
    var transaction = JsonSerializer.Deserialize<StockTransaction>(message);
    
    Console.WriteLine($"Received transaction: {transaction.Id} Amount: {transaction.Amount} shares");
    
    //Check
    var ownership = new StockOwnership(
        Ulid.NewUlid(),
        transaction.ProcessingId,
        transaction.BuyingUserId, 
        transaction.CompanyStockId,
        transaction.PricePerShare,
        transaction.Amount);

    mqClient.Publish(
        "",
        "ses-ownership",
        JsonSerializer.Serialize(ownership),
        null);
};

mqClient.RegisterCallback("ses-transaction", messageCallback);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(mqClient);

var app = builder.Build();

app.Run();