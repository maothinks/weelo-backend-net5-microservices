using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Weelo.Microservices.Properties.Domain;
using Weelo.Microservices.Properties.Infrastructure.Data.Configs;

namespace Weelo.Microservices.Properties.Infrastructure.Data.Contexts
{
    public class PropertyContext : DbContext
    {
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyImage> PropertyImages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            IConfigurationBuilder configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfiguration configuration = configBuilder.Build();
            options.UseSqlServer(configuration.GetConnectionString("defaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new PropertyConfig());
        }
    }
}
