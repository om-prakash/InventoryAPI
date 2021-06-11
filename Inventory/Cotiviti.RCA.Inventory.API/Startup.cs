using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Test.Inventory.API.Enums;
using Test.Inventory.API.Interfaces;
using Test.Inventory.API.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;

namespace Test.Inventory.API.Template
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private IWebHostEnvironment CurrentEnvironment { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddSingleton<ILog, Logger>();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = Configuration["ApplicationName"], Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
               {
                 new OpenApiSecurityScheme
                 {
                   Reference = new OpenApiReference
                   {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                   }
                  },
                  new string[] { }
                }
              });
            });

          }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILog logger)
        {
            CurrentEnvironment = env;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Configure global exception handler to handle unhandled exceptions
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        // log unhandled exception
                        var exceptionFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        await logger.Log(LogTypeEnum.Error, "An unhandled exception occurred in " + Configuration["ApplicationName"], exceptionFeature?.Error);
                        // return an error response to clients
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            Message = "Oops, an unhandled exception occurred."
                        }));
                    });
                });
                app.UseHsts();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", Configuration["ApplicationName"] + " V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors("AllowAllHeaders");

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
