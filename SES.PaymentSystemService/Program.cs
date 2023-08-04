using PaymentSystemService;
using SES.Shared.Infrastructure;

var mqClient = MqClientFactory
    .CreateMqClient(MqClients.RabbitMQ, "ses-rabbitmq");

IHostBuilder builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHttpClient();
        services.AddSingleton(mqClient);
        services.AddHostedService<Worker>();
    });

IHost host = builder.Build();
host.Run();