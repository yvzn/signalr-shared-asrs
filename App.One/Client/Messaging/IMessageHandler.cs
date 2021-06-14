using Microsoft.AspNetCore.SignalR.Client;

namespace App.One.Client.Messaging
{
    public interface IMessageHandler
    {
        void Listen(HubConnection hubConnection);
    }
}
