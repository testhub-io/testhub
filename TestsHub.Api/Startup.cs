using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO.Compression;
using TestHub.Api.ApiDataProvider;

namespace TestHub.Api
{
    public class Startup
    {
        readonly string AllowTestsHubOrigins = "AllowTestsHubOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var context = new TestHub.Data.DataModel.TestHubDBContext(configuration);
            context.Database.Migrate();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();            

            services.AddCors(options => options.AddPolicy(AllowTestsHubOrigins,
                builder =>
                       builder.WithOrigins("http://testshub.com", "http://testhub-frontend-dev.s3-website.us-east-2.amazonaws.com",
                        "https://test-hub.io", "http://test-hub.io", "https://test-hub-frontend.azurewebsites.net",
                        // TODO: remove when on prod
                        "http://localhost:8080")

                           .AllowAnyHeader()
                           .AllowAnyMethod()));

            
            // compression
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
            services.AddRazorPages();
            services.AddSingleton(Configuration);
            services.AddScoped<IDataProviderFactory, DataProviderFactory>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Testhub API", Version = "v1" });                                
                
            });
            services.ConfigureSwaggerGen(options =>
            {
#if DEBUG
                options.IncludeXmlComments("TestsHub.Api.xml");
#elif RELEASE
                options.IncludeXmlComments("TestsHub.Api.xml");
#endif
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // TODO: Disable for prod
            app.UseDeveloperExceptionPage();

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
            app.UseResponseCompression();
            app.UseRouting();
            app.UseAuthorization();
            //app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();                
            });
        }
    }
}
