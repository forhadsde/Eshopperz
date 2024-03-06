using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Eshopperz.Controllers;

// Define the namespace for the Program class within the Eshopperz.Models namespace.
namespace Eshopperz.Models
{
    // Declare a public class named Program.
    public class Program
    {
        // Define the entry point for the application.
        public static void Main(string[] args)
        {
            // Create a web application builder with the provided command-line arguments.
            var builder = WebApplication.CreateBuilder(args);

            // Add services to configure API Explorer for endpoints.
            builder.Services.AddEndpointsApiExplorer();
            
            // Add Swagger generation services.
            builder.Services.AddSwaggerGen();
            
            // Add services for MVC controllers.
            builder.Services.AddControllers();
            
            // Add Entity Framework DbContext with SQLite as the database provider.
            builder.Services.AddDbContext<EshopperzContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("Connection")));

            // Add identity services with Entity Framework stores and default token providers.
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<EshopperzContext>().AddDefaultTokenProviders();

            // Configure email settings by binding the configuration section "EmailSettings" to EmailSettings class.
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            // Add scoped services for EmailService and RolesController.
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<RolesController>();

            // Configure JWT authentication.
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Set token validation parameters for JWT authentication.
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            // Add controllers with views and configure JSON options for preserving references.
            builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                });

            // Build the web application.
            var app = builder.Build();

            // Configure the HTTP request pipeline.

            // If not in development mode, configure exception handling, HSTS, and HTTPS redirection.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // If in development mode, enable Swagger and Swagger UI.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Map controllers, enable HTTPS redirection, serve static files, and configure routing.
            app.MapControllers();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            // Map the default controller route and run the application.
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

