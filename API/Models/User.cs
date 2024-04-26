using Microsoft.AspNetCore.Identity;

namespace API.Models;

public class User : IdentityUser<int>
{
    public UserAddress Address { get; set; }
}
