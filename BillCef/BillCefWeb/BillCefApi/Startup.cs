using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using BillCefApi.DAL;
using Microsoft.EntityFrameworkCore;
using MySql.Data.EntityFrameworkCore.Infraestructure;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BillCefApi
{
    public static class MySQLDbContextOptionsExtensions
    {
        public static DbContextOptionsBuilder UseMySQL(this DbContextOptionsBuilder optionsBuilder,
            string connectionString,
            Action<MySQLDbContextOptionsBuilder> MySQLOptionsAction = null)
        {
            var extension = optionsBuilder.Options.FindExtension<MySQLOptionsExtension>();
            if (extension == null)
                extension = new MySQLOptionsExtension();
            extension.ConnectionString = connectionString;

            IDbContextOptionsBuilderInfrastructure o = optionsBuilder as IDbContextOptionsBuilderInfrastructure;
            o.AddOrUpdateExtension(extension);

            MySQLOptionsAction?.Invoke(new MySQLDbContextOptionsBuilder(optionsBuilder));

            return optionsBuilder;
        }
    }
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            //services.AddDbContext<MySqlContext>(options=> options.u)
            string connectionString = Configuration.GetConnectionString("ConnectString");

            services.AddDbContext<MySqlContext>(options =>
                options.UseMySQL(connectionString)
            );

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}
