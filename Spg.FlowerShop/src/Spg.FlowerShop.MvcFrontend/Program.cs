using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Spg.FlowerShop.Application.Products;
// using Spg.FlowerShop.Infrastructure;
using Spg.FlowerShop.Repository.Products;
using Spg.FlowerShop.Core;
using Spg.FlowerShop.Infrastructure;
using Spg.FlowerShop.Domain.Interfaces;
using Spg.FlowerShop.Domain.Model;
using Spg.FlowerShop.Repository;

namespace Spg.FlowerShop.MvcFrontend
{
    public class Program
    {
        public static void Main(string[] args)
       {
            var builder = WebApplication.CreateBuilder(args);

            string? connectionString = builder.Configuration.GetConnectionString("MyConnection");

            // Add services to the container builder.Services
            builder.Services.AddControllersWithViews();

            // Add Services to IServiceCollection
            builder.Services.ConfigureServices();

            // Add Repositories to IServiceCollection
            builder.Services.ConfigureRepositories();

            builder.Services.ConfigureSqLite(connectionString); // Extension Method ConfigureSqLite

            var app = builder.Build(); // Web App wird gestellt

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}