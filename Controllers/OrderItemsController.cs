using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshopperz.Models;
// Define the namespace for the OrderItemController class within the Eshopperz.Controllers namespace.
namespace Eshopperz.Controllers
{
    // Declare a public class named OrderItemController that inherits from ControllerBase.
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        // Private field to store the database context injected through the constructor.
        private readonly EshopperzContext _context;

        // Constructor to initialize the database context.
        public OrderItemController(EshopperzContext context)
        {
            _context = context;
        }

        // Action to retrieve all order items.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems()
        {
            // Retrieve and return all order items from the database.
            return await _context.OrderItems.ToListAsync();
        }

        // Action to retrieve a specific order item by its composite key values.
        [HttpGet("{productId}/{orderId}/{dateOfOrder}")]
        public async Task<ActionResult<OrderItem>> GetOrderItem(int productId, int orderId, DateOnly dateOfOrder)
        {
            // Finding the order item entry in the database based on the provided composite key values.
            var orderProduct = await _context.OrderItems.FindAsync(productId, orderId, dateOfOrder);

            if (orderProduct == null)
            {
                // Returning a NotFound response if the order item with the provided composite key values cannot be found.
                return NotFound($"An OrderItem with ProductId {productId}, OrderId {orderId}, and DateOfOrder {dateOfOrder} does not match the composite key values.");
            }

            // Returning the found order item entry.
            return orderProduct;
        }

        // Action to update an existing order item.
        [HttpPut("{productId}/{orderId}/{dateOfOrder}")]
        public async Task<IActionResult> PutOrderItem(int productId, int orderId, DateOnly dateOfOrder, OrderItem orderProduct)
        {
            // Checking if the provided data matches the composite key values of the order item.
            if (
                productId != orderProduct.ProductId ||
                orderId != orderProduct.OrderId ||
                dateOfOrder != orderProduct.DateOfOrder)
            {
                // Returning a BadRequest response if the provided data does not match the composite key values.
                return BadRequest("The provided data does not match the composite key values.");
            }

            // Setting the state of the order item object in the context to Modified.
            _context.Entry(orderProduct).State = EntityState.Modified;

            try
            {
                // Saving changes to the database.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemExists(orderProduct.OrderItemId))
                {
                    // Returning a NotFound response if the order item with the provided composite key values cannot be found.
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Returning a NoContent response indicating successful update of the order item.
            return NoContent();
        }

        // Action to create a new order item.
        [HttpPost]
        public async Task<ActionResult<OrderItem>> PostOrderItem(OrderItem orderProduct)
        {
            // Checking if an order item with the same composite key values already exists.
            if (await _context.OrderItems.AnyAsync(op =>
                op.ProductId == orderProduct.ProductId &&
                op.OrderId == orderProduct.OrderId &&
                op.DateOfOrder == orderProduct.DateOfOrder))
            {
                // Returning a Conflict response if an order item with the same composite key values already exists.
                return Conflict($"An order product with  ProductId {orderProduct.ProductId}, OrderId {orderProduct.OrderId}, and DateOfOrder {orderProduct.DateOfOrder} already exists.");
            }

            // Check if the ProductId exists.
            if (!await _context.Products.AnyAsync(p => p.ProductId == orderProduct.ProductId))
            {
                // Returning a Conflict response if the ProductId does not exist in the Product Table.
                return Conflict($"Product with ID {orderProduct.ProductId} not found and it does not exist in Product Table");
            }

            // Check if the OrderId exists.
            if (!await _context.Orders.AnyAsync(o => o.OrderId == orderProduct.OrderId))
            {
                // Returning a Conflict response if the OrderId does not exist in the Order Table.
                return Conflict($"Order with ID {orderProduct.OrderId} not found and it does not exist in Order Table");
            }

            // Add the new order item to the context.
            _context.OrderItems.Add(orderProduct);

            // Save changes to the database.
            await _context.SaveChangesAsync();

            // Return a 201 Created response with the created order item.
            return CreatedAtAction("GetOrderItem", new { id = orderProduct.OrderItemId }, orderProduct);
        }

        // Action to delete a specific order item by its ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(int id)
        {
            // Finding the order item entry in the database based on the provided ID.
            var orderProduct = await _context.OrderItems.FindAsync(id);
            if (orderProduct == null)
            {
                // Returning a NotFound response if the order item with the provided ID cannot be found.
                return NotFound();
            }

            // Removing the order item entry from the context and saving changes to the database.
            _context.OrderItems.Remove(orderProduct);
            await _context.SaveChangesAsync();

            // Returning a NoContent response indicating successful deletion of the order item.
            return NoContent();
        }

        // Private method to check if an order item with a given ID exists.
        private bool OrderItemExists(int id)
        {
            return _context.OrderItems.Any(e => e.OrderItemId == id);
        }
    }
}