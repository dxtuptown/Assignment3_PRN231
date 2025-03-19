using BusinessObject;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class MyDBContext : IdentityDbContext<AppUser>
    {
        public MyDBContext()
        {
        }
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);
            IConfiguration configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Cấu hình Identity với AppUser

            modelBuilder.Entity<OrderDetail>()
                .HasKey(ba => new { ba.OrderID, ba.ProductID });

            modelBuilder.Entity<OrderDetail>()
                .HasOne(ba => ba.Order)
                .WithMany(b => b.OrderDetails)
                .HasForeignKey(ba => ba.OrderID);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(ba => ba.Product)
                .WithMany(a => a.OrderDetails)
                .HasForeignKey(ba => ba.ProductID);
        }
    }
}
