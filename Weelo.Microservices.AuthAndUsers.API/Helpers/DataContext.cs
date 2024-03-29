using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Weelo.Microservices.AuthAndUsers.API.Entities;

namespace Weelo.Microservices.AuthAndUsers.API.Helpers
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<User> Users { get; set; }
    }
}