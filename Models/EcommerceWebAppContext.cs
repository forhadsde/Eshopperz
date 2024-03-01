using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
namespace EcommerceWebApp.Models
{
    public class EcommerceWebAppContext
    {
        public EcommerceWebAppContext(DbContextOptions<EcommerceWebAppContext> options) : base(options)
        {
        }
        public System.Data.Entity.DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(p => p.OrderItem)
            .HasForeignKey(p => p.CategoryId)
            .IsRequired();

            modelBuilder.Entity<Category>()
            .HasOne(c => c.CategoryName)
            
            modelBuilder.Entity<OrderItem>()
                .Haskey(op => new {op.ProductId, op.OrderId, op.ProductOrder});

            modelBuilder.Entity<OrderItem>()
                .HasOne(op => op.Product)
                .WithMany()
                .HasForeignKey(op => op.ProductId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(op => op.Order).WithMany().HasForeignKey(op => op.OderId);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.CartId)
                .WithOne()
                .HasForeignKey(c => c.CustomerId);

            modelBuilder.Entity<Order>()
            .HasOne(o => o.OrderId)
            .WithMany(o => o.OrderItem)
            .HasForeignKey(o => o.CustomerId);
 

            modelBuilder.Entity<Admin>()
            .HasOne(a => a.AdminId)
            .WithMany(a => a.Product)
            .IsRequired();






        }


    }
}
