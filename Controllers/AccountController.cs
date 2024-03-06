// Import necessary namespaces.
using Eshopperz.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// Define the namespace for the AccountController class within the Eshopperz.Controllers namespace.
namespace Eshopperz.Controllers
{
    // Declare a public class named AccountController that inherits from ControllerBase.
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        // Private fields to store dependencies injected through constructor.
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;

        // Constructor to initialize dependencies.
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, EmailService emailService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        // Action for user registration.
        [HttpPost("register")]
        public async Task<IActionResult> Register(AuthModel model)
        {
            // Create a new IdentityUser using the provided email and password.
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            // Check if user registration is successful.
            if (result.Succeeded)
            {
                // Generate an email verification token.
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                // Create the verification link.
                var verificationLink = Url.Action("VerifyEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);

                // Send the verification email.
                var emailSubject = "Email Verification";
                var emailBody = $"Please verify your email by clicking the following link: {verificationLink}";
                _emailService.SendEmail(user.Email, emailSubject, emailBody);

                return Ok("User registered successfully. An email verification link has been sent.");
            }

            return BadRequest(result.Errors);
        }

        // Action to handle email verification.
        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return Ok("Email verification successful.");
            }

            return BadRequest("Email verification failed.");
        }

        // Action for user login.
        [HttpPost("login")]
        public async Task<IActionResult> Login(AuthModel model)
        {
            // Attempt to sign in the user using provided email and password.
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            // Check if login attempt is successful.
            if (result.Succeeded)
            {
                // Retrieve the user and roles.
                var user = await _userManager.FindByEmailAsync(model.Email);
                var roles = await _userManager.GetRolesAsync(user);

                // Generate JWT token.
                var token = GenerateJwtToken(user, roles);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid login attempt.");
        }

        // Action for user logout.
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logged out");
        }

        // Private method to generate JWT token.
        private string GenerateJwtToken(IdentityUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Add roles as claims.
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Create JWT token using configuration settings.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddHours(Convert.ToDouble(_configuration["Jwt:ExpireHours"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Action to delete a user account.
        [HttpDelete]
        public async Task<IActionResult> DeleteAccount(string userId)
        {
            // Find the user by user ID.
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest("User not found.");
            }

            // Delete the user account.
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                // Optionally, you can sign the user out if they are logged in.
                // await _signInManager.SignOutAsync();

                return Ok("User account deleted successfully.");
            }

            // If account deletion fails, return the error messages.
            return BadRequest(result.Errors);
        }
    }
}
