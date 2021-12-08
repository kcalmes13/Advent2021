using Autofac;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnionWireless.Utilities.Logging.Infrastructure.Modules;
using UnionWireless.Utilities.ModelParser.Api.Domain;
using UnionWireless.Utilities.ModelParser.Api.Helper;
using UnionWireless.Utilities.Validation.Infrastructure.Modules;

namespace UnionWireless.Utilities.ModelParser.Api
{
    /// <summary>
    /// Configurations for asp .net core
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// List of apis for application
        /// </summary>
        private readonly List<ApiInfo> apis = new();

        /// <summary>
        /// Configurations for this app
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">list of services to add to</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = ServiceFabricHelper.GetUrlForStatefulService(Environment.GetEnvironmentVariable("Authority"));
                    options.ApiName = Environment.GetEnvironmentVariable("ApiName");
                    options.RequireHttpsMetadata = false;
                });

            services.AddApiVersioning(config =>
            {
                config.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(
            options =>
            {
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddCors();
            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                // Add a swagger document for each discovered API version  
                foreach (var api in apis)
                {
                    options.SwaggerDoc(api.GroupName, new OpenApiInfo
                    {
                        Title = $"{api.Name} {api.Version}",
                        Version = api.Version,
                        Description = $"{api.Description} {(api.Depricated ? " DEPRECATED" : string.Empty)}"
                    });
                }

                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                // Set the comments path for the Swagger JSON and UI.
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { Environment.GetEnvironmentVariable("ApiName") }
                    }
                });
            });
        }

        /// <summary>
        /// ConfigureContainer is where you can register things directly
        /// with Autofac.This runs after ConfigureServices so the things
        /// here will override registrations made in ConfigureServices.
        /// Don't build the container, that gets done for you by the factory.
        /// </summary>
        /// <param name="builder"></param>
        // ReSharper disable once UnusedMember.Global
        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Register any regular dependencies.
            builder.RegisterModule<LoggingModule>();
            builder.RegisterModule(new ValidationModule(Assembly.GetExecutingAssembly()));
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">application builder</param>
        /// <param name="env">system environment</param>
        /// <param name="provider">Api versioning provider</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            var scheme = Environment.GetEnvironmentVariable("WebEnvironment")!.ToLower();

            app.UseCors(options =>
            {
                options.WithOrigins(Environment.GetEnvironmentVariable("CORS_Origins")!.Split(','))
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowCredentials()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.          
            app.UseSwagger(options =>
            {
                options.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    if (swagger.Servers.Count >= 1)
                    {
                        swagger.Servers[0].Url = $"{scheme}://{httpReq.Host}";
                    }
                });
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    apis.Add(new ApiInfo()
                    {
                        GroupName = description.GroupName,
                        Name = "Model Parser Utility",
                        Depricated = description.IsDeprecated,
                        Version = description.ApiVersion.ToString(),
                        Description = "Api for making it so users only get data they are authorized to"
                    });

                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", "Model Parser Utility " + description.ApiVersion);
                }

                c.RoutePrefix = string.Empty; // Puts Swagger at root of web app.
            });

            if (scheme.Equals("https"))
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}