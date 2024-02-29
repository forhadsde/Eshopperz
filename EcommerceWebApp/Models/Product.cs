using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceWebApp.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public double? Price { get; set; }

        [ForeignKey("Category")]

        public int CategoryId { get; set; }
        public Category? Category { get; set; }


    }
}
