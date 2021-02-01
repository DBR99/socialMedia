using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SocialMedia.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        //En este create host builder se cargan los appsettings.json variables de entorno etc

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
