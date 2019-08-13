// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityServer
{
    public class Startup
    {
		public IConfiguration Configuration { get; }
		public IHostingEnvironment Environment { get; }

        public Startup(IHostingEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
			Configuration = configuration;
		}

        public void ConfigureServices(IServiceCollection services)
        {
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;

            services.AddCors(setup =>
            {
                setup.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.WithOrigins("http://localhost:8082");
                    policy.AllowCredentials();
                });
            });

            services.AddMvcCore()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1)
                .AddJsonFormatters();

            var builder = services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "http://localhost:8082/index.html";
                options.UserInteraction.ErrorUrl = "http://localhost:8082/error.html";
                options.UserInteraction.LogoutUrl = "http://localhost:8082/logout.html";

				if(!string.IsNullOrEmpty(Configuration["Issuer"]))
				{
					options.IssuerUri = Configuration["Issuer"];
				}
			})
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetTestUsers());

            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("need to configure key material");
            }

            var cors = new DefaultCorsPolicyService(new LoggerFactory().CreateLogger<DefaultCorsPolicyService>())
            {
                AllowAll = true
            };
            services.AddSingleton<ICorsPolicyService>(cors);
            services.AddTransient<IReturnUrlParser, ReturnUrlParser>();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to support static files
            //app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseCors();

            // uncomment, if you want to add an MVC-based UI
            app.UseMvc();
        }
    }
}
