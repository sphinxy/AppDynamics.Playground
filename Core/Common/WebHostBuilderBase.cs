using System.Collections.Generic;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Common
{
    public class WebHostBuilderBase
    {
        public static IWebHostBuilder BuildWebHost(string[] args, int port) =>
            new WebHostBuilder()
                .ConfigureAppConfiguration(builder =>
                    builder.AddJsonFile("appsettings.json", false))
                .UseKestrel(o => o.Listen(IPAddress.Any, port)).UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<StartupWithAkka>();
    }
}