using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SigmaAWS.NetCore2.DAL.Context;
//using SigmaAWS.NetCore2.DAL.Context;
using SigmaAWS.NetCore2.DAL.Repositories;
using SigmaAWS.NetCore2.Domain.Interfaces;
using Swashbuckle.AspNetCore.Swagger;

namespace SigmaAWS.NetCore2.API
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
            //Add SQL Server support
            services.AddDbContext<CustomersContext>(options =>
            {
                options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Customers;Trusted_Connection=True;");
            });

            ////Add SqLite support
            //services.AddDbContext<CustomersContext>(options =>
            //{
            //    options.UseSqlite(@"data source=C:\Users\lulo8295\Source\SigmaAWS\SigmaAWS.NetCore2.API\SigmaAWS.NetCore2.API\Data\Customers.sqlite");
            //});

            services.AddMvc().AddJsonOptions(
                options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );

            //Handle XSRF Name for Header
            services.AddAntiforgery(options => {
                options.HeaderName = "X-XSRF-TOKEN";
            });

            InjectSwaggerAsService(services);

            RegisterServices(services);
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    
                    name: "DefaultAPi",
                    template: "api/{controller}/{id?}"
                    );
            });

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "SigmaAWS API v1");
            });
        }
        
        private static void InjectSwaggerAsService(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "ASP.NET Core Customers API",
                    Description = "ASP.NET Core/Swagger Documentation",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "SigmaITC", Url = "http://www.sigmaitc.se/sv/" },
                    License = new License { Name = "MIT", Url = "https://en.wikipedia.org/wiki/MIT_License" }
                });
            });
        }


        private static void RegisterServices(IServiceCollection services)
        {
            //DAL
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<CustomersContext>();
        }
    }
}
