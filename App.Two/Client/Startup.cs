using App.Two.Client.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Two.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions<ClientOptions>()
                .BindConfiguration(ClientOptions.SectionName);

            services
                .AddHostedService<ConnectToSignalRHostedService>();

            services
                .AddTransient<IMessageHandler, MessageHandler>();
        }

        public void Configure()
        {

        }
    }
}
