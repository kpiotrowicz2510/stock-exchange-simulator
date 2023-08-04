using System.Text;
using RabbitMQ.Client.Events;

namespace SES.Shared.Infrastructure;

public class BasicMessageHandler
{
    private readonly IMqClient _mqClient;
    private List<Action<string>> _callBacks = new();

    public BasicMessageHandler(IMqClient mqClient)
    {
        _mqClient = mqClient;
    }

    public void RegisterCallback(Action<string> callback)
    {
        _callBacks.Add(callback);
    }

    public void GenericHandler(object? model, BasicDeliverEventArgs ea)
    {
        try
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            
            _callBacks.ForEach(x=>x(message));
    
            _mqClient.SendAck(ea.DeliveryTag);
        }
        catch (Exception ex)
        {
            _mqClient.SendNack(ea.DeliveryTag);
            //Log
        }
    }
}