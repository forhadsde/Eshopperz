
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
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
