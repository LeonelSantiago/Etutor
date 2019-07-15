using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Etutor.Core.Extensions
{
    public static class IdentityExtensions
    {

        public static IdentityBuilder AddSecondIdentity<TUser>(this IServiceCollection services, Action<IdentityOptions> setupAction = null)
        where TUser : class
        {
            /*This is the same way Microsoft does it within the AddIdentity 
            inside IdentityServiceCollectionExtension*/
            services.AddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
            services.AddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
            services.AddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
            services.AddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipalFactory<TUser>>();
            services.AddScoped<ISecurityStampValidator, SecurityStampValidator<TUser>>();
            services.AddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<TUser>>();
            services.AddScoped<UserManager<TUser>, AspNetUserManager<TUser>>();
            services.AddScoped<SignInManager<TUser>, SignInManager<TUser>>();

            if (setupAction != null)
                services.Configure(setupAction);

            return new IdentityBuilder(typeof(TUser), services);
        }

        public static string ToMessage(this IEnumerable<IdentityError> errors)
        {
            var result = new StringBuilder();
            foreach (var error in errors)
            {
                result.AppendLine(error.Description);
            }
            return result.ToString();
        }
    }
}
