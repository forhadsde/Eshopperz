using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceWebApp.Models
{
    public class Cart
    {
        public int CartId { get; set; }

       

        [ForeignKey("Customer")]

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}
