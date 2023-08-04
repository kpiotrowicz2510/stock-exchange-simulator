namespace SES.Shared.Infrastructure;

public enum MqClients
{
    RabbitMQ = 0,
}

public static class MqClientFactory
{
    public static IMqClient CreateMqClient(MqClients type, string hostname)
    {
        return type switch
        {
            MqClients.RabbitMQ => new RabbitMqClient(hostname),
            _ => throw new ArgumentException("Broker not supported")
        };
    }
}