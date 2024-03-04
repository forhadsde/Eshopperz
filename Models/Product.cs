using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Eshopperz.Models
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
