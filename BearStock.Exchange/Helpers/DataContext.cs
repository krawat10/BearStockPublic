using System;
using BearStock.Exchange.Entities.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BearStock.Exchange.Helpers
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
#if DEBUG   
            //options.UseSqlServer("Data Source=localhost;Initial Catalog=master;Integrated Security=True");
            options.UseInMemoryDatabase(nameof(DataContext));
#else
            options.UseSqlServer(Configuration.GetConnectionString("DataContext"));
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Quota>()
                .HasOne(quota => quota.Exchange)
                .WithMany(exchange => exchange.Quotas)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }

        public DbSet<Quota> Quotas { get; set; }
        public DbSet<Entities.Database.Exchange> Exchanges { get; set; }

    }
}