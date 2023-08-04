using System.Text.Json;
using SES.Shared.Contract;
using SES.Shared.Extensions;
using SES.Shared.Infrastructure;

Ulid SystemUserId = Ulid.NewUlid();

var mqClient = MqClientFactory
    .CreateMqClient(MqClients.RabbitMQ, "ses-rabbitmq");

Action<string> messageCallback = (message) =>
{
    var transaction = JsonSerializer.Deserialize<StockOwnership>(message);
    
    Console.WriteLine($"Received ownership: {transaction.Id} Amount: {transaction.Amount} shares");
    
    Console.WriteLine("Saving");
};

mqClient.RegisterCallback("ses-ownership", messageCallback);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(mqClient);

var app = builder.Build();

app.Run();