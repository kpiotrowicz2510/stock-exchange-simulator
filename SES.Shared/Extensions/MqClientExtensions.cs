using SES.Shared.Infrastructure;

namespace SES.Shared.Extensions;

public static class MqClientExtensions
{
    public static IMqClient RegisterCallback(this IMqClient client, string queue, Action<string> callback)
    {
        var messageHandler = new BasicMessageHandler(client);
        
        messageHandler.RegisterCallback(callback);
        
        client.Subscribe(queue, messageHandler.GenericHandler, false);
        
        return client;
    }
}