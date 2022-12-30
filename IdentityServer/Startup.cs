﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // uncomment, if you want to add an MVC-based UI
            services.AddControllersWithViews();

            var builder = services.AddIdentityServer(                
                options =>
                {
                    options.AccessTokenJwtType = "JWT";
                    options.EmitLegacyResourceAudienceClaim = true;
                })
                .AddInMemoryIdentityResources(Config.Ids)
                .AddInMemoryApiResources(Config.Apis)
                .AddInMemoryClients(Config.Clients)
                .AddTestUsers(TestUsers.Users);

            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            services.AddAuthentication()
                .AddGoogle("Google", 
                    options =>
                    {
                        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                        options.ClientId = "855494373025-c6554kbvibhqv2at9i4il80p5emn857t.apps.googleusercontent.com";
                        options.ClientSecret = "3V6XZiQ8fyvur7zD0DFO_SjP";
                    })
                .AddSalesforce(
                    options =>
                    {
                        options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                        options.ClientId = "3MVG95jctIhbyCpohqVZ1ZPeJshQK5lqjavRU4KOb_WVX4AjU7pA8SLgzAzTbqfVcNRvcoHn6IbJVy1yuBnol";
                        options.ClientSecret = "C8C63BFBF4BF5132674F7CB585C770006C4D7C26B972E4940F54A187923A9D8E";
                    });

            // uncomment if you want to have login UI in spa
            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    //CoreApi 프로젝트 web api 
                    policy.WithOrigins("https://localhost:44321")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to have login UI in spa
            //app.UseCors("default");

            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
