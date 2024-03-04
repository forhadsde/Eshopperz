using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Eshopperz.Models;
using System.Collections.Generic;

namespace Eshopperz.Models
{
    public class EshopperzContext : IdentityDbContext<IdentityUser>
    {
        public EshopperzContext(DbContextOptions<EshopperzContext> options) : base(options)
        {
        }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany()  // Assuming you have a navigation property 'Products' in your Category class
            .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<Category>()
                .HasIndex(c => c.CategoryName) // Create an index for the Name property
                .IsUnique();

            //many to many relationship configuration
            modelBuilder.Entity<OrderItem>()
                .HasKey(OrderItem => new { OrderItem.OrderId, OrderItem.ProductId, OrderItem.ProductOrderDate  });
            
            modelBuilder.Entity<OrderItem>()
            .HasOne(OrderItem => OrderItem.Product)
            .WithMany()
            .HasForeignKey(OrderItem => OrderItem.ProductId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(OrderItem => OrderItem.Order)
            .WithMany()
            .HasForeignKey(OrderItem => OrderItem.OrderId);

            // Other entity configurations...

            base.OnModelCreating(modelBuilder);
        }
        


    }
}