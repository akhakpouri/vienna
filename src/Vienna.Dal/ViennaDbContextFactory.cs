using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace Vienna.Dal
{
    public class ViennaDbContextFactory : IDesignTimeDbContextFactory<ViennaDbContext>
    {
        public ViennaDbContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            var config = builder.Build();

            var optionBuildr = new DbContextOptionsBuilder<ViennaDbContext>();
            optionBuildr.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            Console.WriteLine($"DesigntimeDbContextFactory: using connection string: {config.GetConnectionString("DefaultString")}");

            return new ViennaDbContext(optionBuildr.Options);
        }
    }
}
