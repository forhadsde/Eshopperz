using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;
using Eshopperz.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Serilog;

namespace Eshopperz.Models
{
    /// <summary>
    /// The main class responsible for configuring and running the web application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The entry point for the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            // Set up the initial logger configuration, writing logs to a file and console.
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Add services for API Explorer, Swagger, and MVC controllers.
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();
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
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}