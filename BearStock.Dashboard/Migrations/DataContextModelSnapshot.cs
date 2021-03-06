// <auto-generated />
using System;
using BearStock.Dashboard.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BearStock.Dashboard.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BearStock.Dashboard.Entities.Dashboard", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Uuid");

                    b.ToTable("Dashboards");
                });

            modelBuilder.Entity("BearStock.Dashboard.Entities.Stock", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<Guid>("DashboardUuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Ticket")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Uuid");

                    b.HasIndex("DashboardUuid");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("BearStock.Dashboard.Entities.StockPosition", b =>
                {
                    b.Property<Guid>("Uuid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("NEWID()");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<double>("PricePerShare")
                        .HasColumnType("float");

                    b.Property<long>("SharesAmount")
                        .HasColumnType("bigint");

                    b.Property<Guid>("StockUuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Uuid");

                    b.HasIndex("StockUuid");

                    b.ToTable("StockPositions");
                });

            modelBuilder.Entity("BearStock.Dashboard.Entities.Stock", b =>
                {
                    b.HasOne("BearStock.Dashboard.Entities.Dashboard", "Dashboard")
                        .WithMany("Stocks")
                        .HasForeignKey("DashboardUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dashboard");
                });

            modelBuilder.Entity("BearStock.Dashboard.Entities.StockPosition", b =>
                {
                    b.HasOne("BearStock.Dashboard.Entities.Stock", "Stock")
                        .WithMany("StockPositions")
                        .HasForeignKey("StockUuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stock");
                });

            modelBuilder.Entity("BearStock.Dashboard.Entities.Dashboard", b =>
                {
                    b.Navigation("Stocks");
                });

            modelBuilder.Entity("BearStock.Dashboard.Entities.Stock", b =>
                {
                    b.Navigation("StockPositions");
                });
#pragma warning restore 612, 618
        }
    }
}
