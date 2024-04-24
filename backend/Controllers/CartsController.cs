// Import necessary namespaces.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshopperz.Models;
using Serilog;

// Define the namespace for the CartsController class within the eshopperz namespace.
namespace eshopperz.Controllers
{
    // Declare a public class named CartsController that inherits from ControllerBase.
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        // Private field to store the database context injected through the constructor.
        private readonly EshopperzContext _context;

        // Constructor to initialize the database context.
        public CartsController(EshopperzContext context)
        {
            _context = context;
        }

        // Action to retrieve all carts.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCarts()
        {
            try
            {
                return await _context.Carts.ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving all carts.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to retrieve a specific cart by its ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            try
            {
                var cart = await _context.Carts.FindAsync(id);

                if (cart == null)
                {
                    return NotFound();
                }

                return cart;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while retrieving cart with ID {id}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to update an existing cart.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            try
            {
                if (id != cart.CartId)
                {
                    return BadRequest();
                }

                _context.Entry(cart).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                return Ok(new { message = $"Cart with CartID {cart.CartId} updated successfully." });
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while updating cart with ID {id}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to create a new cart.
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            try
            {
                var existingCustomer = await _context.Customers.FindAsync(cart.CustomerId);
                if (existingCustomer == null)
                {
                    Log.Warning($"Customer with the provided ID {cart.CustomerId} does not exist. Cart creation failed.");
                    return Conflict($"Customer with the provided {cart.CustomerId} does not exist.");
                }

                var existingCart = await _context.Carts.FindAsync(cart.CartId);
                if (existingCart != null)
                {
                    Log.Warning($"A cart with the provided CartId {cart.CartId} already exists. Cart creation failed.");
                    return Conflict($"A cart with the provided CartId {cart.CartId} already exists.");
                }

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCart", new { id = cart.CartId }, cart);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during cart creation.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to delete a specific cart by its ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            try
            {
                var cart = await _context.Carts.FindAsync(id);
                if (cart == null)
                {
                    Log.Warning($"A Cart with the ID {id} cannot be found. Cart deletion failed.");
                    return Conflict($"A Cart with the ID {id} cannot be found.");
                }

                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while deleting cart with ID {id}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Private method to check if a cart with a given ID exists.
        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }
    }
}