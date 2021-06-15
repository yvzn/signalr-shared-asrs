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
            var configurationSection = Configuration.GetSection(ServerOptions.SectionName);
            var applicationName = configurationSection.Get<ServerOptions>().AppName;

            services
                .AddOptions<ServerOptions>()
                .Bind(configurationSection);

            services
                .AddSignalR()
                .AddAzureSignalR(
                 // this is required to discriminate messages between apps
                 options => options.ApplicationName = applicationName
                );

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
