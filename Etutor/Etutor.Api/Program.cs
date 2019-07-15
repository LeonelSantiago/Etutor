using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Etutor.Core.Extensions;
using Etutor.DataModel;

namespace Etutor.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Seed(DbInitializer.Init).Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
