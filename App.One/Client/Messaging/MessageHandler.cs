using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.One.Client.Messaging
{
    internal class MessageHandler : IMessageHandler
    {
        private readonly ClientOptions options;
        private readonly ILogger<MessageHandler> logger;

        public MessageHandler(
            IOptions<ClientOptions> options,
            ILogger<MessageHandler> logger)
        {
            this.options = options.Value;
            this.logger = logger;
        }

        public void Listen(HubConnection hubConnection)
        {
            hubConnection.On<string>("pingFromServer", OnPing);
        }

        private void OnPing(string serverName)
        {
            logger.LogInformation("{ApplicationName} recieves ping from {ServerName}", options.AppName, serverName);
        }
    }
}
