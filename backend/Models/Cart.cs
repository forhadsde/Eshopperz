using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


// Define the namespace for the Cart class within the Eshopperz.Models namespace.
namespace Eshopperz.Models
{
    // Declare a public class named Cart.
    public class Cart
    {
        // Define a property representing the CartId. It is an integer.
        public int CartId { get; set; }

        // Decorate the CustomerId property with the ForeignKey attribute, specifying "Customer" as the foreign key.
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        // Define a property representing the associated Customer. It allows null values.
        public Customer? Customer { get; set; }
    }
}
