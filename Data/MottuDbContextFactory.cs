MottuDbContextFactory:
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MottuApi.Data
{
    public class MottuDbContextFactory : IDesignTimeDbContextFactory<MottuDbContext>
    {
        public MottuDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MottuDbContext>();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("OracleConnection");

            optionsBuilder.UseOracle(connectionString);

            return new MottuDbContext(optionsBuilder.Options);
        }
    }
}
