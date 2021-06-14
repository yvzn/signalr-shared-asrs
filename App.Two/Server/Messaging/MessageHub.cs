﻿using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace App.Two.Server.Messaging
{
    internal class MessageHub : Hub
    {
        private readonly ILogger<MessageHub> logger;
        private readonly ServerOptions options;

        public MessageHub(IOptions<ServerOptions> options, ILogger<MessageHub> logger)
        {
            this.logger = logger;
            this.options = options.Value;
        }

        public async Task MessageFromClient(string clientName, string message)
        {
            logger.LogInformation("{ApplicationName} received message from {ClientName}: {ClientMessage}", options.AppName, clientName, message);
            await Clients.All.SendAsync("messageFromServer", message);
        }
    }
}
