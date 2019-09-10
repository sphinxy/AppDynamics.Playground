using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;

namespace Common
{
    public class CustomWebHostService : WebHostService
    {
        public static void Launch(string[] args, int port)
        {
            bool isService = !(Debugger.IsAttached || args.Contains("--console"));

            var pathToContentRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            if (isService)
            {
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                pathToContentRoot = Path.GetDirectoryName(pathToExe);
            }

            var host = WebHostBuilderBase.BuildWebHost(args, port)
                .UseContentRoot(pathToContentRoot)
                .Build();


            if (isService)
            {
               host.RunAsCustomService();
            }
            else
            {
                host.Run();
            }
        }

        private readonly IWebHost _webHost;

        private Timer _timer;

        private async void OnTimer(object state)
        {
            System.Diagnostics.Debugger.Launch();
            try
            {
                await new AsyncBackgroundCaller(_webHost.Services.GetService<IHttpClientFactory>()).Call(Addresses.Amazon, $"/");
            }
            catch (Exception e)
            {
                Debugger.Break();
            }
        }

        //works only when runs as service, not from debug in VS
        protected override void OnStarting(string[] args)
        {
            _timer = new Timer(OnTimer, new object(), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5));
            base.OnStarting(args);
        }

        public CustomWebHostService(IWebHost host) : base(host)
        {
            _webHost = host;
        }

        protected override void OnStopping()
        {
            _timer.Dispose();
            base.OnStopping();
        }
    }

    public static class CustomCustomWebHostServiceExtensions
    {
        public static void RunAsCustomService(this IWebHost host)
        {
            var webHostService = new CustomWebHostService(host);
            ServiceBase.Run(webHostService);
        }
    }
}