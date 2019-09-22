using Etutor.DataModel.Context;
using Etutor.DataModel.SampleData;
using System.Linq;
using System;
using Etutor.DataModel.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Etutor.DataModel
{
    public static class DbInitializer
    {
        public async static Task Init(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<ApplicationDbContext>();
            await context.Database.EnsureCreatedAsync();

            var userManager = serviceProvider.GetService<UserManager<User>>();
            if (!userManager.Users.Any())
            {
                foreach (var usuario in UsuarioSample.Usuarios)
                {
                    await userManager.CreateAsync(usuario);
                }
            }

        }
    }
}
