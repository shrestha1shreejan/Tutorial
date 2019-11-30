using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CoreAPI.Helpers;
using DataLibrary;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ModelsLibrary;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace CoreAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            ///
            services.Configure<CosmosDbConfigurationManager>(Configuration.GetSection("CosmosDBConfiguration"));

            //DI
            services.AddDataLibrary();            

            // cors
            services.AddCors( c => {
                c.AddPolicy("CustomPolicy", options => {
                    options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            // security
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.
                        GetBytes(Configuration.GetSection("Tokens:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };               
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Global exception handler 
                app.UseExceptionHandler(builder => {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            // TODO: Modify the response with our own extension method
                            // Added extension method for this
                            context.Response.AddApplicationError(error.Error.Message);// will get rid of misleading cors error
                            await context.Response.WriteAsync(error.Error.Message);
                            
                        }
                    });
                });

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("CustomPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
