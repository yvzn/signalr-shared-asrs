using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.One.Client.Messaging
{
    internal class ConnectToSignalRHostedService : BackgroundService
    {
        private readonly ClientOptions options;
        private readonly IMessageHandler messageHandler;
        private readonly ILogger<ConnectToSignalRHostedService> logger;
        private HubConnection? hubConnection;

        public ConnectToSignalRHostedService(
            IMessageHandler messageHandler,
            IOptions<ClientOptions> options,
            ILogger<ConnectToSignalRHostedService> logger)
        {
            this.options = options.Value;
            this.messageHandler = messageHandler;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            InitHubConnection();
            ConfigureMessageHandler();
            await ConnectAsync(stoppingToken);
        }

        private HubConnection InitHubConnection()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(options.ServerHub)
                .Build();

            return hubConnection;
        }

        private async Task ConnectAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (hubConnection is not null)
                {
                    logger.LogDebug("{ApplicationName} connecting to {HubUrl} ...", options.AppName, options.ServerHub?.AbsoluteUri.ToString());

                    await hubConnection.StartAsync(cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{ApplicationName} failed to connect to SignalR Hub", options.AppName);
            }
        }

        private void ConfigureMessageHandler()
        {
            if (hubConnection is not null)
            {
                messageHandler.Listen(hubConnection);
            }
        }
    }
}
