// Import necessary namespaces.
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eshopperz.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using Newtonsoft.Json; // Add this import for JsonConvert
using Serilog;

// Define the namespace for the RolesController class within the Eshopperz.Controllers namespace.
namespace Eshopperz.Controllers
{
    // Declare a public class named RolesController that inherits from ControllerBase.
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class RolesController : ControllerBase
    {
        // Private fields to store the role and user managers injected through the constructor.
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        // Constructor to initialize the role and user managers.
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // Action to retrieve all roles.
        [HttpGet]
        public IActionResult GetRoles()
        {
            try
            {
                // Retrieve all roles from the role manager.
                var roles = _roleManager.Roles.ToList();

                // Return the list of roles.
                return Ok(roles);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while retrieving all roles.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to retrieve a specific role by its ID.
        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRole(string roleId)
        {
            try
            {
                // Finding the role in the role manager based on the provided ID.
                var role = await _roleManager.FindByIdAsync(roleId);

                if (role == null)
                {
                    // Returning a NotFound response if the role with the provided ID cannot be found.
                    return NotFound("Role not found.");
                }

                // Return the found role.
                return Ok(role);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while retrieving role with ID {roleId}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to check if a user is in the "Administrator" role.
        public async Task<bool> IsUserInAdminRole(string userId)
        {
            try
            {
                // Retrieve the user.
                var user = await _userManager.FindByIdAsync(userId);

                // Check if the user exists.
                if (user != null)
                {
                    // Check if the user is in the "Administrator" role.
                    return await _userManager.IsInRoleAsync(user, "Administrator");
                }

                // Return false if the user doesn't exist or if an error occurred.
                return false;
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while checking if user with ID {userId} is in Admin role.");
                return false;
            }
        }

        // Action to create a new role.
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.RoleName))
                {
                    return BadRequest("RoleName is required.");
                }

                // Create a new identity role.
                var role = new IdentityRole(request.RoleName);

                // Attempt to create the role using the role manager.
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return Ok("Role created successfully.");
                }

                // Return errors if role creation fails.
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred during role creation.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to update an existing role.
        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleModel model)
        {
            try
            {
                // Find the role in the role manager based on the provided role ID.
                var role = await _roleManager.FindByIdAsync(model.RoleId);

                if (role == null)
                {
                    // Returning a NotFound response if the role with the provided ID cannot be found.
                    return NotFound("Role not found.");
                }

                // Update the role name.
                role.Name = model.NewRoleName;

                // Attempt to update the role using the role manager.
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return Ok("Role updated successfully.");
                }

                // Return errors if role update fails.
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while updating role with ID {model.RoleId}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to delete a specific role by its ID.
        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            try
            {
                // Find the role in the role manager based on the provided role ID.
                var role = await _roleManager.FindByIdAsync(roleId);

                if (role == null)
                {
                    // Returning a NotFound response if the role with the provided ID cannot be found.
                    return NotFound("Role not found.");
                }

                // Attempt to delete the role using the role manager.
                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return Ok("Role deleted successfully.");
                }

                // Return errors if role deletion fails.
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while deleting role with ID {roleId}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // Action to assign a role to a user.
        [HttpPost("assign-role-to-user")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleModel model)
        {
            try
            {
                // Find the user in the user manager based on the provided user ID.
                var user = await _userManager.FindByIdAsync(model.UserId);

                // Log incoming request body.
                Console.WriteLine("Incoming Request Body: " + JsonConvert.SerializeObject(model));

                if (user == null)
                {
                    // Returning a NotFound response if the user with the provided ID cannot be found.
                    return NotFound("User not found.");
                }

                // Check if the role exists.
                var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);

                if (!roleExists)
                {
                    // Returning a NotFound response if the role does not exist.
                    return NotFound("Role not found.");
                }

                // Attempt to add the user to the specified role using the user manager.
                var result = await _userManager.AddToRoleAsync(user, model.RoleName);

                if (result.Succeeded)
                {
                    return Ok("Role assigned to user successfully.");
                }

                // Return errors if assigning the role to the user fails.
                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while assigning role to user with ID {model.UserId}.");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
