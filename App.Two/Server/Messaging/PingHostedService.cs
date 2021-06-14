using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace App.Two.Server.Messaging
{
    internal class PingHostedService : BackgroundService, IDisposable
    {
        private readonly ServerOptions options;
        private readonly IHubContext<MessageHub> hubContext;
        private readonly ILogger<PingHostedService> logger;
        private readonly Timer timer;

        public PingHostedService(
            IHubContext<MessageHub> hubContext,
            IOptions<ServerOptions> options,
            ILogger<PingHostedService> logger)
        {
            this.options = options.Value;
            this.hubContext = hubContext;
            this.logger = logger;

            timer = new Timer(SendPing);
        }

        private void SendPing(object? state)
        {
            _ = SendPingAsync(state);
        }

        private async Task SendPingAsync(object? _)
        {
            logger.LogInformation("{ApplicationName} sends ping", options.AppName);
            await hubContext.Clients.All.SendAsync("pingFromServer", options.AppName);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            // TODO wait for Azure SignalR connected before scheduling
            timer.Change(options.Ping.DueTime, options.Ping.Period);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            timer.Dispose();
            base.Dispose();
        }
    }
}
