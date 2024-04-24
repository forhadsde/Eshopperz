using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// Define the namespace for the Customer class within the Eshopperz.Models namespace.
namespace Eshopperz.Models
{
    // Declare a public class named Customer.
    public class Customer
    {
        // Define a property representing the CustomerId. It is an integer.
        public int CustomerId { get; set; }

        // Define a property representing the CustomerName. It allows null values.
        public string? CustomerName { get; set; }

        // Define a property representing the Email. It allows null values.
        public string? Email { get; set; }
    }
}
