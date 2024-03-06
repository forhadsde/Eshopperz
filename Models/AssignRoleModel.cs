// Define the namespace for the AssignRoleModel class within the Eshopperz.Models namespace.
namespace Eshopperz.Models
{
    // Declare a public class named AssignRoleModel.
    public class AssignRoleModel
    {
        // Define a property representing the user ID. It allows null values.
        public string? UserId { get; set; }
        
        // Define a property representing the role name. It allows null values.
        public string? RoleName { get; set; }
    }
}