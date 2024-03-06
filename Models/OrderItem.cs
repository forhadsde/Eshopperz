using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;




// Define the namespace for the OrderItem class within the Eshopperz.Models namespace.
namespace Eshopperz.Models
{
    // Declare a public class named OrderItem.
    public class OrderItem
    {
        // Define a property representing the OrderProductId. It is an integer.
        public int OrderItemId { get; set; }

        // Decorate the ProductId property with the ForeignKey attribute, specifying "Product" as the foreign key.
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        // Define a property representing the associated Product. It allows null values.
        public Product? Product { get; set; }

        // Decorate the OrderId property with the ForeignKey attribute, specifying "Order" as the foreign key.
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        // Define a property representing the associated Order. It allows null values.
        public Order? Order { get; set; }

        // Define a property representing the ProductOrderDate. It is of type DateOnly.
        public DateOnly DateOfOrder { get; set; }

        // Define a property representing the Quantity. It is an integer.
        public int Quantity { get; set; }
    }
}
