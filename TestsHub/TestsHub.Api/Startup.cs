using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using TestsHub.Data;

namespace TestsHub.Api
{
    public class Startup
    {
        readonly string AllowTestsHubOrigins = "AllowTestsHubOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var context = new TestsHub.Data.DataModel.TestHubDBContext(configuration);           
            context.Database.Migrate();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors(options => options.AddPolicy(AllowTestsHubOrigins,
                builder=>
                       builder.WithOrigins("http://testshub.com", "https://testshub.com")
                           .AllowAnyHeader()
                           .AllowAnyMethod()));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
            services.AddRazorPages();
            services.AddSingleton(Configuration);
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();             
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.EnvironmentName.Equals("Development", StringComparison.InvariantCultureIgnoreCase))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseCors(AllowTestsHubOrigins);
            app.UseRouting();
            app.UseAuthorization();
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
