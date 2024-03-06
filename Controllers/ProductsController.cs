using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshopperz.Models;
using Microsoft.AspNetCore.Authorization;

// Define the namespace for the ProductController class within the Eshopperz.Controllers namespace.
namespace Eshopperz.Controllers
{
    // Declare a public class named ProductController that inherits from ControllerBase.
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // Private field to store the database context injected through the constructor.
        private readonly EshopperzContext _context;

        // Constructor to initialize the database context.
        public ProductController(EshopperzContext context)
        {
            _context = context;
        }

        // Action to retrieve all products with associated categories.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            // Retrieve all products with associated categories from the database.
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            
            // Return the list of products.
            return products;
        }

        // Action to retrieve a specific product by its ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            // Finding the product entry in the database based on the provided ID.
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                // Returning a Conflict response if the product with the provided ID cannot be found.
                return Conflict($"A product with the ID {id} cannot be found");
            }

            // Return the found product.
            return product;
        }

        // Action to update an existing product.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            // Checking if the provided ID matches the ProductId of the product object.
            if (id != product.ProductId)
            {
                // Returning a BadRequest response if the IDs do not match.
                return BadRequest();
            }

            // Setting the state of the product object in the context to Modified.
            _context.Entry(product).State = EntityState.Modified;

            try
            {
                // Saving changes to the database.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    // Returning a NotFound response if the product with the provided ID cannot be found.
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Returning an Ok response indicating successful update of the product.
            // Also providing a message indicating the changes made.
            return Ok(new { message = $"Changes made to ProductId {product.ProductId}" });
        }

        // Action to create a new product.
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            // Check if a product with the same name already exists.
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductName == product.ProductName);

            // Creating a list of products to display to the user the list of existing products.
            var products = await _context.Products.Include(p => p.Category).ToListAsync();

            if (existingProduct != null)
            {
                // Returning a Conflict response if a product with the same name already exists.
                return Conflict(new { Message = $"A product with the name '{product.ProductName}' already exists in the category '{existingProduct.Category.CategoryName}'.", ExistingProducts = products });
            }

            // No existing product with the same name, proceed with adding the new product.
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Returning a response indicating successful creation of the product.
            return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
        }

        // Action to delete a specific product by its ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // Finding the product entry in the database based on the provided ID.
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                // Returning a Conflict response if the product with the provided ID cannot be found.
                return Conflict($"A product with the ID {id} cannot be found");
            }

            // Removing the product entry from the context and saving changes to the database.
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            // Returning a response indicating successful deletion of the product.
            return NoContent();
        }

        // Private method to check if a product with a given ID exists.
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}