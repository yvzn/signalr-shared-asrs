using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Two.Client.Messaging
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
            ConfigureReconnection();
            ConfigureMessageHandler();
            await ConnectAsync(stoppingToken);
        }

        private HubConnection InitHubConnection()
        {
            hubConnection = new HubConnectionBuilder()
                .WithUrl(options.ServerHub)
                .WithAutomaticReconnect()
                .Build();

            return hubConnection;
        }

        private void ConfigureReconnection()
        {
            if (hubConnection is not null)
            {
                hubConnection.Closed += ReconnectAsync;
            }
        }

        private async Task ReconnectAsync(Exception error)
        {
            // wait some time before reconnecting
            await Task.Delay(5_000);

            await ConnectAsync(CancellationToken.None);
        }

        private async Task ConnectAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (hubConnection is not null)
                {
                    logger.LogDebug("{ApplicationName} connecting to {HubUrl} ...", options.AppName, options.ServerHub?.AbsoluteUri.ToString());

                    await hubConnection.StartAsync(cancellationToken);

                    logger.LogInformation("{ApplicationName} connected to SignalR Hub", options.AppName);
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
