
namespace EcommerceWebApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }


        public Customer? Customer { get; set; }

        public List<Product>? Products { get; set; }

        public int? CartId { get; set; }

        public Cart? Cart { get; set; }
    }
}
