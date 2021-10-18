using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BearStock.Dashboard.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.Dashboard>()
                .HasKey(dashboard => dashboard.Uuid);
            modelBuilder.Entity<Entities.Dashboard>()
                .Property(dashboard => dashboard.Uuid)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Entities.Dashboard>()
                .HasMany(c => c.Stocks)
                .WithOne(e => e.Dashboard)
                .HasForeignKey(stock => stock.DashboardUuid)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entities.Stock>()
                .HasKey(stock => stock.Uuid);
            modelBuilder.Entity<Entities.Stock>()
                .Property(stock => stock.Uuid)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Entities.Stock>()
                .HasMany(c => c.StockPositions)
                .WithOne(e => e.Stock)
                .HasForeignKey(stock => stock.StockUuid)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entities.StockPosition>()
                .HasKey(position => position.Uuid);
            modelBuilder.Entity<Entities.StockPosition>()
                .Property(position => position.Uuid)
                .HasDefaultValueSql("NEWID()");
        }

        public DbSet<Entities.Dashboard> Dashboards { get; set; }
        public DbSet<Entities.Stock> Stocks { get; set; }
        public DbSet<Entities.StockPosition> StockPositions { get; set; }
    }
}