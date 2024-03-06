// Define the namespace for the AuthModel class within the Eshopperz.Models namespace.
namespace Eshopperz.Models
{
    // Declare a public class named AuthModel.
    public class AuthModel
    {
        // Define a property representing the email. It allows null values.
        public string? Email { get; set; }

        // Define a property representing the password. It allows null values.
        public string? Password { get; set; }
    }
}