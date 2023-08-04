using SES.Shared.Infrastructure;
using UserManagementService;

var mqClient = MqClientFactory
    .CreateMqClient(MqClients.RabbitMQ, "ses-rabbitmq");

IHostBuilder builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton(mqClient);
        services.AddHostedService<Worker>();
    });

IHost host = builder.Build();
host.Run();
