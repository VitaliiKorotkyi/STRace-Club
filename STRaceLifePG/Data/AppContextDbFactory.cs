using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using StreetRaceLifeVK.Data;
using System.IO;
namespace STRaceLifePG.Data
{
    public class AppContextDbFactory : IDesignTimeDbContextFactory<AppContextDb>
    {
        public AppContextDb CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppContextDb>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);

            return new AppContextDb(optionsBuilder.Options);
        }
    }
}
