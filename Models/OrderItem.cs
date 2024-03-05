using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;




namespace Eshopperz.Models
{
    public class OrderItem
    {
         public int OrderProductId { get; set; }
        
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        [ForeignKey("Order")]
        public int OrderId { get ; set; }
        public Order? Order{ get ;set; }
        public DateOnly ProductOrderDate { get; set; }
        
        public int Quantity { get; set; }
    }
}
