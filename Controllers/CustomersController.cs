using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshopperz.Models;
// Define the namespace for the CustomerController class within the Eshopperz.Controllers namespace.
namespace Eshopperz.Controllers
{
    // Declare a public class named CustomerController that inherits from ControllerBase.
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        // Private field to store the database context injected through the constructor.
        private readonly EshopperzContext _context;

        // Constructor to initialize the database context.
        public CustomerController(EshopperzContext context)
        {
            _context = context;
        }

        // Action to retrieve all customers.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            // Retrieve and return all customers from the database.
            return await _context.Customers.ToListAsync();
        }

        // Action to retrieve a specific customer by its ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            // Finding the customer entry in the database based on the provided ID.
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                // Returning a NotFound response if the customer with the provided ID cannot be found.
                return NotFound();
            }

            // Returning the found customer entry.
            return customer;
        }

        // Action to update an existing customer.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            // Checking if the provided ID matches the CustomerId of the customer object.
            if (id != customer.CustomerId)
            {
                // Returning a BadRequest response if the IDs do not match.
                return BadRequest();
            }

            // Marking the customer object as Modified in the context.
            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                // Saving changes to the database.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    // Returning a NotFound response if the customer with the provided ID cannot be found.
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Returning an Ok response indicating successful update of the customer.
            // Also providing a message indicating the changes made.
            return Ok(new { message = $"Changes made to account with CustomerId {customer.CustomerId}" });
        }

        // Action to create a new customer.
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            // Adding the customer object to the context.
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Returning a response indicating successful creation of the customer.
            // Also providing the URI for accessing the newly created customer.
            return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
        }

        // Action to delete a specific customer by its ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            // Finding the customer entry in the database based on the provided ID.
            var customer = await _context.Customers.FindAsync(id);

            if (customer == null)
            {
                // Returning a Conflict response if the customer with the provided ID cannot be found.
                return Conflict($"A Customer with the ID {customer.CustomerId} cannot be found");
            }

            // Removing the customer from the context and saving changes to the database.
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            // Returning a NoContent response indicating successful deletion of the customer.
            return NoContent();
        }

        // Private method to check if a customer with a given ID exists.
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
