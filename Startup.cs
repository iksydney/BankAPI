using BankAPI.Data;
using BankAPI.Services;
using BankAPI.Services.Implementations;
using BankAPI.Services.Interfaces;
using BankAPI.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAPI
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
            services.AddDbContext<BankDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Path"));
            });
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITransactionService, TransactionService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Bank API",
                    Version = "v1",
                    Description = "A Bank API",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Ogbonna Ikenna",
                        Email = "cindionline@yahoo.com",
                        Url = new Uri("https://github.com/iksydney/BankAPI")
                    }
                });
            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                var prefix = string.IsNullOrEmpty(options.RoutePrefix) ? "." : "..";
                options.SwaggerEndpoint($"{prefix}/swagger/v1/swagger.json", "Bank API");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
