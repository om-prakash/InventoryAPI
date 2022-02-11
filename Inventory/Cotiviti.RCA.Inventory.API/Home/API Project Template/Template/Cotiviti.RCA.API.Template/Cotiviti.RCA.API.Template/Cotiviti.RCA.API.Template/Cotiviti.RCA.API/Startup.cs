using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using $safeprojectname$.Enums;
using $safeprojectname$.Interfaces;
using $safeprojectname$.Utility;
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

namespace $safeprojectname$.Template
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

            // Configure CORS to allow cross origin calls from JavaScript
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder
                               .WithOrigins(Configuration["AllowOrigins"].Split(new char[] { ',' }))
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                      });
            });
            // Configure ASP.NET Core MVC
            //services.AddMvc()
            //        .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);


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
        

        // JWT Token authentication
        // TODO: After Cotiviti decided a final authentication and authorization service, below code
        // needs to be changed accordingly. 
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters =
                       new TokenValidationParameters
                       {
                           ValidateIssuer = false,
                           ValidateAudience = false,
                           SignatureValidator =
                           delegate (string token, TokenValidationParameters parameters)
                           {
                               var jwt = new JwtSecurityToken(token);
                               try
                               {
                                   bool result;
                                   using (var httpClient = new HttpClient())
                                   {
                                       httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                                       using (Task<HttpResponseMessage> response = httpClient.GetAsync(Configuration["AuthURI"] + "/GetisTokenValid"))
                                       {
                                           Task<string> apiResponse = response.Result.Content.ReadAsStringAsync();
                                           result = JsonConvert.DeserializeObject<bool>(apiResponse.Result);
                                       }
                                   }
                                   if (!result)
                                   {
                                       throw new Exception("Token signature validation failed.");
                                   }
                               }
                               catch (Exception ex)
                               {
                                   throw new Exception("An error occurred while validating JWT token.", ex);
                               }

                               return jwt;
                           }
                       };
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
                        await logger.Log(LogTypeEnum.Error, "An unhandled exception occurred in "+ Configuration["ApplicationName"], exceptionFeature?.Error);
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
