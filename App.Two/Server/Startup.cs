using App.Two.Server.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace App.Two.Server
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
                .AddSignalR()
                .AddAzureSignalR(
                // this is required to discriminate messages between apps
                // options => options.ApplicationName = "serverApp2"
                );

            services
                .AddOptions<ServerOptions>()
                .BindConfiguration(ServerOptions.SectionName);

            services.AddHostedService<PingHostedService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/message-hub");
            });
        }
    }
}
