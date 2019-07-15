using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace Etutor.Core.Extensions
{
    public static class WebHostExtensions
    {
        public static IWebHost Seed(this IWebHost webhost, Func<IServiceProvider, Task> seeder)
        {
            using (var scope = webhost.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var env = scope.ServiceProvider.GetService<IHostingEnvironment>();
                if (env.IsDevelopment())
                    seeder(scope.ServiceProvider).GetAwaiter().GetResult();
            }
            return webhost;
        }
    }
}
