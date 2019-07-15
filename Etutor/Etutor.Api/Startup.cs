using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Globalization;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using StructureMap;
using AutoMapper;
using FluentValidation.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;
using Newtonsoft.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Etutor.Api.Filters;
using Etutor.Core.Models.Configurations;
using Etutor.Core.Middlewares;
using Etutor.BL.Setup;
using Etutor.DataModel.Entities;
using Etutor.DataModel.Context;
using Etutor.BL.Localization;
using Etutor.Core.Models;
using Etutor.Core.Extensions;
using Etutor.BL.Validators;
using Etutor.BL.IoC;
using Etutor.BL.Resources;
using Microsoft.Extensions.Localization;

namespace Etutor.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();
            Configuration = builder.Build();
            Environment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add logging service. 
            services.AddLogging(builder => builder.AddConfiguration(Configuration).AddConsole());

            if (!Environment.IsProduction())
            {
                services.AddHttpsRedirection(options =>
                {
                    options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                    options.HttpsPort = Configuration.GetValue<int>("sslPort");
                });

                services.AddHsts(options =>
                {
                    options.Preload = true;
                    options.IncludeSubDomains = true;
                    options.MaxAge = TimeSpan.FromDays(1);
                });
            }
            // Adds configuration service.
            services.AddSingleton(Configuration);

            // Adds services required for using options.
            services.AddOptions();

            // Register the IConfiguration instance which paths binds against.
            services.Configure<PathsConfig>(Configuration.GetSection(typeof(PathsConfig).Name));
            services.Configure<JwtConfig>(Configuration.GetSection(typeof(JwtConfig).Name));
            services.Configure<AdConfig>(Configuration.GetSection(typeof(AdConfig).Name));
            services.Configure<SmtpConfig>(Configuration.GetSection(typeof(SmtpConfig).Name));

            #region OrionDbContext
            // Add entity framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                // Configure the context to use Microsoft SQL Server.
                options.UseSqlServer(Configuration.GetConnectionString("ApplicationSQLConnection"),
                    b => b.MigrationsAssembly("Etutor.Api"));
            });



            #endregion

            #region Identity, Json Web Token, Auth

            // Register the Identity services.
            services.AddIdentity<Usuario, Rol>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddUserStore<UserStore<Usuario, Rol, ApplicationDbContext, int, UsuarioClaim, UsuarioRol, UsuarioLogin, UsuarioToken, RolClaim>>()
                .AddRoleStore<RoleStore<Rol, ApplicationDbContext, int, UsuarioRol, RolClaim>>()
                .AddDefaultTokenProviders();




            /*
                * Configure Identity to use the same JWT claims as OpenIddict instead of
                * the legacy WS-Federation claims it uses by default (ClaimTypes).
                * Which saves you from doing the mapping in your authorization controller.
            */

            services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            });

            //Add JWT Authentication
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); //remove default claims

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {

                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    ValidIssuer = Configuration["JWTConfig:Issuer"],
                    ValidAudience = Configuration["JWTConfig:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTConfig:Key"])),
                    ClockSkew = TimeSpan.Zero, // remove delay of token when expire
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true
                };
            });

            services.AddAuthorization();
            #endregion

            #region AutoMapper
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfiles("Etutor.BL");
            });
            #endregion

            #region CORS
            // Add CORS services.
            services.AddCors(option =>
            {
                option.AddPolicy("AllowAdminOrigin",
                builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().WithExposedHeaders(typeof(OperationResult).Name.PascalToKebabCase()));
            });
            #endregion

            #region Localization
            // Add Localization service
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            #endregion

            #region OData
            // Add AddOData service
            services.AddOData();
            #endregion

            #region AddMvc, JsonOptions, FluentValidation
            // Create service provider to resolve dependecies of CustomExceptionFilterAttribute 
            // registered by reference  due a exception occurs when DI Container attemp to resolve 
            // its dependecies if the  enviroment variable is set to any value diferent of development
            var serviceProvider = services.BuildServiceProvider();


            // Add framework services.
            services.AddMvc(options =>
            {
                // https://blogs.msdn.microsoft.com/webdev/2018/08/27/asp-net-core-2-2-0-preview1-endpoint-routing/
                // Because conflicts with ODataRouting as of this version
                // could improve performance though
                options.EnableEndpointRouting = false;

                options.Filters.Add(new CustomExceptionFilterAttribute(serviceProvider.GetService<IStringLocalizer<ShareResource>>(), serviceProvider.GetService<ILogger<CustomExceptionFilterAttribute>>()));
                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odataEtutorxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odataEtutorxx-odata"));
                }
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            })
            .AddFluentValidation(Options =>
            {
                Options.RegisterValidatorsFromAssemblyContaining<UsuarioValidator>();
                Options.LocalizationEnabled = true;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            #endregion

            #region Swagger
            // Add AddSwaggerGen service
            services.AddSwaggerGen(cfg =>
            {
                cfg.SwaggerDoc("v1", new Info { Title = "Etutor API", Version = "v1" });
                cfg.AddSecurityDefinition("Bearer", new ApiKeyScheme { In = "header", Description = "Please enter JWT with Bearer into field.", Name = "Authorization", Type = "apiKey" });
                cfg.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                     { "Bearer", Enumerable.Empty<string>() },
                });
            });
            #endregion

            #region StructureMap
            // Built-in dependency injection is replaced by StructureMap.
            var container = new Container();

            container.Configure(config =>
            {
                config.AddRegistry(new ApplicationRegistry(services.BuildServiceProvider()));
                config.Populate(services);
            });
            #endregion
            //
            Debug.WriteLine(container.WhatDidIScan());

            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsProduction())
            {
                app.UseHttpsRedirection();
                app.UseHsts();
            }

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("es"),
                // Formatting numbers, dates, etc.
                SupportedCultures = { new CultureInfo("es"), new CultureInfo("en"), new CultureInfo("fr") },
                // UI strings that we have localized.
                SupportedUICultures = { new CultureInfo("es"), new CultureInfo("en"), new CultureInfo("fr") }
            });


            // Enable middleware to serve generated Swagger as a JSON endpoint.
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            // To serve the Swagger UI at the app's root (http://localhost:<port>/), set the RoutePrefix property to an empty string
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SGI API V1");
                    c.RoutePrefix = string.Empty;
                });

            app.UseAuthentication()
                .UseStaticFiles()
                .UseCors("AllowAdminOrigin")
                .UseMvc(routes =>
                {

                    // The routes for odata are enabled and the queries allowed are configured
                    routes.Select().Expand().Filter().OrderBy().Count().MaxTop(100);
                    routes.MapODataServiceRoute("OData", "odata", ODataSetup.GetEdmModel(app.ApplicationServices));

                    // Workaround: https://github.com/OData/WebApi/issues/1175#issuecomment-353546186
                    routes.EnableDependencyInjection();
                });
        }
    }
}
