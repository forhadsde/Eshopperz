using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Eshopperz.Models;
using System.Collections.Generic;

// Define the namespace for the EshopperzContext class within the Eshopperz.Models namespace.
namespace Eshopperz.Models
{
    // Declare a public class named EshopperzContext that inherits from IdentityDbContext<IdentityUser>.
    public class EshopperzContext : IdentityDbContext<IdentityUser>
    {
        // Constructor that takes DbContextOptions<EshopperzContext> as a parameter and passes it to the base class constructor.
        public EshopperzContext(DbContextOptions<EshopperzContext> options) : base(options)
        {
        }

        // DbSet properties for database tables.
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }

        // Override the OnModelCreating method to configure relationships and indexes.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure a one-to-many relationship between Product and Category.
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId);

            // Create a unique index on the CategoryName property of the Category entity.
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.CategoryName)
                .IsUnique();

            // Configure a composite key for the OrderItem entity.
            modelBuilder.Entity<OrderItem>()
                .HasKey(orderItem => new { orderItem.OrderId, orderItem.ProductId, orderItem.DateOfOrder });

            // Configure a one-to-many relationship between OrderItem and Product.
            modelBuilder.Entity<OrderItem>()
                .HasOne(orderItem => orderItem.Product)
                .WithMany()
                .HasForeignKey(orderItem => orderItem.ProductId);

            // Configure a one-to-many relationship between OrderItem and Order.
            modelBuilder.Entity<OrderItem>()
                .HasOne(orderItem => orderItem.Order)
                .WithMany()
                .HasForeignKey(orderItem => orderItem.OrderId);

            // Call the base class OnModelCreating method.
            base.OnModelCreating(modelBuilder);
        }
    }
}
