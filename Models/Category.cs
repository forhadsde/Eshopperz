using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Eshopperz.Models
{
    public class Category
    {  
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }
        public int OrderItem { get; set; }
    }
}
