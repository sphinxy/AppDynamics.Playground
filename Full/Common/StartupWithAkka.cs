using System.Reflection;
using Akka.Actor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common
{
    public class StartupWithAkka
    {
        public StartupWithAkka(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(ActorSystem.Create("Appdynamics-Akka-Playground"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAsyncBackgroundCaller, AsyncBackgroundCaller>();
            services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
            services.AddMvc().AddApplicationPart(Assembly.GetEntryAssembly());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
 
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
        }

    }
}