// Import necessary namespaces.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Eshopperz.Models;
using Serilog;

// Define the namespace for the CategoryController class within the eshopperz namespace.
namespace eshopperz.Controllers
{
    // Declare a public class named CategoryController that inherits from ControllerBase.
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "Administrator")]
    public class CategoryController : ControllerBase
    {
        // Private field to store the database context injected through the constructor.
        private readonly EshopperzContext _context;

        // Constructor to initialize the database context.
        public CategoryController(EshopperzContext context)
        {
            _context = context;
        }

        // Action to retrieve all categories.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            try
            {
                // Include the associated products for each category
                return await _context.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving all categories.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to retrieve a specific category by its ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            try
            {
                // Finding the category entry in the database based on the provided ID
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    // Returning a NotFound response if the category with the provided ID cannot be found
                    return NotFound();
                }

                // Returning the found category entry
                return category;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while retrieving category with ID {id}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to update an existing category.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            try
            {
                // Checking if the provided ID matches the CategoryId of the category object
                if (id != category.CategoryId)
                {
                    // Returning a BadRequest response if the provided ID does not match the CategoryId
                    return BadRequest();
                }

                // Setting the state of the category object in the context to Modified
                _context.Entry(category).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                // Returning an Ok response indicating successful update of the category
                // Also providing a message indicating the changes made
                return Ok(new { message = $"Changes made to  account with CategoryId {category.CategoryId}" });
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while updating category with ID {id}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to create a new category.
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            try
            {
                // Checking if a category with the same name or ID already exists in the database
                var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == category.CategoryName || c.CategoryId == category.CategoryId);

                // Retrieving all existing categories for reference
                var categories = await _context.Categories.ToListAsync();

                if (existingCategory != null)
                {
                    // Category with the same name already exists, return a conflict response
                    Log.Warning($"A category with the name {category.CategoryName} or with ID {category.CategoryId} already exists. Category creation failed.");
                    return Conflict(new { Message = $"A category with the name {category.CategoryName} or with ID {category.CategoryId}  already exists.", ExistingCategories = categories });
                }

                // No existing category with the same name, proceed with adding the new category
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                // Returning a response indicating successful creation of the category
                return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during category creation.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to delete a specific category by its ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                // Finding the category entry in the database based on the provided ID
                var category = await _context.Categories.FindAsync(id);

                if (category == null)
                {
                    // Returning a NotFound response if the category with the provided ID cannot be found
                    return NotFound();
                }

                // Check if there are any associated products
                var associatedProducts = await _context.Products.Where(p => p.CategoryId == id).ToListAsync();
                if (associatedProducts.Any())
                {
                    // If there are associated products, return a conflict response indicating deletion is not allowed
                    Log.Warning($"Cannot delete category '{category.CategoryId}' because it has associated products. Category deletion failed.");
                    return Conflict($"Cannot delete category '{category.CategoryId}' because it has associated products.");
                }

                // If there are no associated products with the category, proceed with deleting the category
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                // Returning a NoContent response indicating successful deletion of the category
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while deleting category with ID {id}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Private method to check if a category with a given ID exists.
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.CategoryId == id);
        }
    }
}
