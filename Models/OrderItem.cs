

namespace EcommerceWebApp.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public DateOnly ProductOrderDate { get; set; }
        
        public int Quantity { get; set; }
    }
}
