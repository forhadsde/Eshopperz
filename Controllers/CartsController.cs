// Import necessary namespaces.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshopperz.Models;

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
            return await _context.Carts.ToListAsync();
        }

        // Action to retrieve a specific cart by its ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        // Action to update an existing cart.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.CartId)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = $"New Cart Updated with CartID {cart.CartId}" });
        }

        // Action to create a new cart.
        
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            // Checking if the customer associated with the cart exists in the Customer Table
            var existingCustomer = await _context.Customers.FindAsync(cart.CustomerId);
            if (existingCustomer == null)
            {
                // Returning a BadRequest response if the customer does not exist
                return Conflict($"Customer with the provided {cart.CustomerId} does not exist.");
            }
              // Checking if a cart with the provided CartId already exists
            var existingCart = await _context.Carts.FindAsync(cart.CartId);
            if (existingCart != null)
            {
                // Returning a Conflict response if the cart already exists
                return Conflict($"A cart with the provided CartId {cart.CartId} already exists.");
            }

            // Adding the new cart entry to the context
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

             // Returning a response indicating successful creation of the cart
            return CreatedAtAction("GetCart", new { id = cart.CartId }, cart);
        }

        // Action to delete a specific cart by its ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return Conflict($"A Cart with the ID {id} cannot be found");
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Private method to check if a cart with a given ID exists.
        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartId == id);
        }
    }
}