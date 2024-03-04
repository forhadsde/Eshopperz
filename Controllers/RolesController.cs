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
using Newtonsoft.Json;

namespace Eshopperz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        

        [HttpGet("{roleId}")]
        public async Task<IActionResult> GetRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            return Ok(role);
        }

        public async Task<bool> IsUserInAdminRole(string userId)
        {
            // Retrieve the user
            var user = await _userManager.FindByIdAsync(userId);

            // Check if the user exists
            if (user != null)
            {
                // Check if the user is in the "Admin" role
                return await _userManager.IsInRoleAsync(user, "Admin");
            }

            // Return false if the user doesn't exist or if an error occurred
            return false;
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.RoleName))
            {
                return BadRequest("RoleName is required.");
            }

            var role = new IdentityRole(request.RoleName);
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return Ok("Role created successfully.");
            }

            return BadRequest(result.Errors);
        }
       

        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            role.Name = model.NewRoleName;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                return Ok("Role updated successfully.");
            }

            return BadRequest(result.Errors);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                return NotFound("Role not found.");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return Ok("Role deleted successfully.");
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("assign-role-to-user")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            
            Console.WriteLine("Incoming Request Body: " + JsonConvert.SerializeObject(model));

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var roleExists = await _roleManager.RoleExistsAsync(model.RoleName);

            if (!roleExists)
            {
                return NotFound("Role not found.");
            }

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            if (result.Succeeded)
            {
                return Ok("Role assigned to user successfully.");
            }

            return BadRequest(result.Errors);
        }

    }
}