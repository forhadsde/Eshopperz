
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// Define the namespace for the Order class within the Eshopperz.Models namespace.
namespace Eshopperz.Models
{
    // Declare a public class named Order.
    public class Order
    {
        // Define a property representing the OrderId. It is an integer.
        public int OrderId { get; set; }

        // Define a property representing the CustomerId. It is an integer.
        public int CustomerId { get; set; }

        // Define a property representing the associated Customer. It allows null values.
        public Customer? Customer { get; set; }

        // Define a property representing a list of associated Products. It allows null values.
        public List<Product>? Products { get; set; }

        // Decorate the CartId property with the ForeignKey attribute, specifying "Cart" as the foreign key.
        [ForeignKey("Cart")]
        public int? CartId { get; set; }

        // Define a property representing the associated Cart. It allows null values.
        public Cart? Cart { get; set; }
    }
}
