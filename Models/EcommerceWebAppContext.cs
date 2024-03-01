using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
namespace EcommerceWebApp.Models
{
    public class EcommerceWebAppContext : DbContext
    {
        public EcommerceWebAppContext(DbContextOptions<EcommerceWebAppContext> options) : base(options)
        {
        }
        public System.Data.Entity.DbSet<OrderItem> OrderItems { get; set; }
        public System.Data.Entity.DbSet<Cart> Carts { get; set; }
        public System.Data.Entity.DbSet<Category> Categories { get; set; }

        public System.Data.Entity.DbSet<Customer> Customers { get; set; }
        public System.Data.Entity.DbSet<Order> Orders { get; set; }

        public System.Data.Entity.DbSet<Product> Products { get; set; }




    }
}
