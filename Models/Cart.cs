using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Eshopperz.Models
{
    public class Cart
    {
        public int CartId { get; set; }

       

        [ForeignKey("Customer")]

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
