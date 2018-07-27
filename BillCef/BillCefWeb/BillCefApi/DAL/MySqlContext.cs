using BillCefApi.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BillCefApi.DAL
{
    public class MySqlContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
            string ConnectString = config.GetValue<string>("ConnectString");
            optionsBuilder.UseMySQL(ConnectString);
        }

        public virtual DbSet<Plugs> plugs { get; set; }
    }
}
