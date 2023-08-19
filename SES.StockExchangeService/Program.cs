using MediatR;
using Microsoft.EntityFrameworkCore;
using SES.Shared.Infrastructure;
using StockExchangeService;
using StockExchangeService.Persistence;

var mqClient = MqClientFactory
    .CreateMqClient(MqClients.RabbitMQ, "ses-rabbitmq");

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddJsonFile("appsettings.json");
var configuration = configurationBuilder.Build();

IHostBuilder builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContext<CommandDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CommandConnection")), ServiceLifetime.Transient);
        services.AddDbContext<QueryDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("QueryConnection")), ServiceLifetime.Transient);
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        services.AddSingleton(mqClient);
        services.AddHostedService<BackgroundWorker>();
    });

IHost host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<CommandDbContext>().Database.Migrate();
    scope.ServiceProvider.GetRequiredService<QueryDbContext>().Database.Migrate();
}

host.Run();