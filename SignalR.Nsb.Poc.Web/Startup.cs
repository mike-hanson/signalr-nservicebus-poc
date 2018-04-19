using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using SignalR.Nsb.Poc.NServiceBus;
using SignalR.Nsb.Poc.NServiceBus.Abstractions;
using SignalR.Nsb.Poc.Web.Abstractions;
using SignalR.Nsb.Poc.Web.Api;
using SignalR.Nsb.Poc.Web.Endpoints;
using SignalR.Nsb.Poc.Web.Hubs;

namespace SignalR.Nsb.Poc.Web
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
            services.AddSingleton<IOrderHubMessageDispatcher, OrderHubMessageDispatcher>();
            services.AddTransient<IEndpointInstanceBuilder, EndpointInstanceBuilder>();
            services.AddSingleton<IOrderEndpoint, OrderEndpoint>();
            services.AddSingleton<IUserIdentityProvider, UserIdentityProvider>();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseSignalR(routes => { routes.MapHub<OrderHub>("/hubs/order"); })
                .UseMvc();
        }
    }
}
