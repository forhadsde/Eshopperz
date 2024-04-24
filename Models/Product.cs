using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// Define the namespace for the Product class within the Eshopperz.Models namespace.
namespace Eshopperz.Models
{
    // Declare a public class named Product.
    public class Product
    {
        // Define a property representing the ProductId. It is an integer.
        public int ProductId { get; set; }

        // Define a property representing the ProductName. It allows null values.
        public string? ProductName { get; set; }

        // Define a property representing the Price. It allows null values and is of type double.
        public double? Price { get; set; }

        // Define a property representing the quantity. It is an integer.
        public int Quantity { get; set; }

        // Decorate the CategoryId property with the ForeignKey attribute, specifying "Category" as the foreign key.
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Define a property representing the associated Category. It allows null values.
        public Category? Category { get; set; }
    }
}

