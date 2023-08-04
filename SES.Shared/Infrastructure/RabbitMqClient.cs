using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SES.Shared.Infrastructure;

public class RabbitMqClient : IMqClient
{
    private readonly IConnection _connection;
    private readonly IModel? _connectionChannel;
    private bool _isSubscribed = false;
    
    public RabbitMqClient(string connectionHost)
    {
        var factory = new ConnectionFactory { HostName = connectionHost };
        _connection = factory.CreateConnection();
        _connectionChannel = _connection.CreateModel();
        _connectionChannel.BasicQos(0,1, true);
    }

    public void Publish(string? exchange, string routingKey, string message, object? basicProperties)
    {
        var body = Encoding.UTF8.GetBytes(message);

        _connectionChannel.BasicPublish(exchange: exchange,
            routingKey: routingKey,
            basicProperties: basicProperties as IBasicProperties,
            body: body);
    }

    public void Subscribe(string queue, EventHandler<BasicDeliverEventArgs> handler, bool autoAck = true)
    {
        var consumer = new EventingBasicConsumer(_connectionChannel);
        consumer.Received += handler;
        
        _connectionChannel.BasicConsume(queue: queue,
            autoAck: autoAck,
            consumer: consumer);
    }

    public void SendAck(ulong deliveryTag)
    {
        _connectionChannel.BasicAck(deliveryTag, false);
    }
    
    public void SendNack(ulong deliveryTag, bool requeue = true)
    {
        _connectionChannel.BasicNack(deliveryTag, false, requeue);
    }

    public void Dispose()
    {
        _connectionChannel?.Dispose();
        _connection.Dispose();
        
        GC.SuppressFinalize(this);
    }
}