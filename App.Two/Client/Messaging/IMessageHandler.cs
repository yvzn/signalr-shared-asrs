using Microsoft.AspNetCore.SignalR.Client;

namespace App.Two.Client.Messaging
{
    public interface IMessageHandler
    {
        void Listen(HubConnection hubConnection);
    }
}
