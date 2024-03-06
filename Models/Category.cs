using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

// Define the namespace for the Category class within the Eshopperz.Models namespace.
namespace Eshopperz.Models
{
    // Declare a public class named Category.
    public class Category
    {
        // Define a property representing the CategoryId. It is an integer.
        public int CategoryId { get; set; }

        // Define a property representing the CategoryName. It allows null values.
        public string? CategoryName { get; set; }
    }
}
