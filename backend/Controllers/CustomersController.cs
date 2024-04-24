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
            try
            {
                // Retrieve and return all customers from the database.
                return await _context.Customers.ToListAsync();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving all customers.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to retrieve a specific customer by its ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            try
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
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while retrieving customer with ID {id}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to update an existing customer.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            try
            {
                // Checking if the provided ID matches the CustomerId of the customer object.
                if (id != customer.CustomerId)
                {
                    // Returning a BadRequest response if the IDs do not match.
                    return BadRequest();
                }

                // Marking the customer object as Modified in the context.
                _context.Entry(customer).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                // Returning an Ok response indicating successful update of the customer.
                // Also providing a message indicating the changes made.
                return Ok(new { message = $"Changes made to account with CustomerId {customer.CustomerId}" });
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while updating customer with ID {id}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to create a new customer.
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            try
            {
                // Adding the customer object to the context.
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();

                // Returning a response indicating successful creation of the customer.
                // Also providing the URI for accessing the newly created customer.
                return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during customer creation.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to delete a specific customer by its ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                // Finding the customer entry in the database based on the provided ID.
                var customer = await _context.Customers.FindAsync(id);

                if (customer == null)
                {
                    // Returning a Conflict response if the customer with the provided ID cannot be found.
                    Log.Warning($"A Customer with the ID {customer.CustomerId} cannot be found.");
                    return Conflict($"A Customer with the ID {id} cannot be found");
                }

                // Removing the customer from the context and saving changes to the database.
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                // Returning a NoContent response indicating successful deletion of the customer.
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while deleting customer with ID {id}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Private method to check if a customer with a given ID exists.
        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
