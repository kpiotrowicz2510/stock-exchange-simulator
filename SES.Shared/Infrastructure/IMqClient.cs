using RabbitMQ.Client.Events;

namespace SES.Shared.Infrastructure;

public interface IMqClient : IDisposable
{
    void Publish(string? exchange, string routingKey, string message, object? basicProperties);
    void Subscribe(string queue, EventHandler<BasicDeliverEventArgs> handler, bool autoAck);
    void SendAck(ulong deliveryTag);
    void SendNack(ulong deliveryTag, bool requeue = true);
}