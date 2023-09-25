using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Spg.FlowerShop.Domain.Interfaces;
using Spg.FlowerShop.Infrastructure;
using Spg.FlowerShop.Application.Products;
using Spg.FlowerShop.Domain.Model;
using Spg.FlowerShop.Repository.Products;
using Spg.FlowerShop.Repository;
using Spg.FlowerShop.Application.ProductCategories;
using Spg.FlowerShop.Application.Customers;

namespace Spg.FlowerShop.Core
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Meine SQLite-Methode
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        public static void ConfigureSqLite(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<FlowerShopContext>(optionsBuilder =>
            {
                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseSqlite(connectionString);
                }
            });
        }

        // Extension Method für alle Services
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddTransient<IReadOnlyProductService, ProductService>();
            services.AddTransient<IAddableProductService, ProductService>();
            services.AddTransient<IReadOnlyGenericProductCategoryService, ProductCategoryService>();
            services.AddTransient<IDeletableProductService, ProductService>();
            services.AddTransient<IUpdateableProductService, ProductService>();

            
            //services.AddTransient<IReadOnlyCustomerService, CustomerService>();
            //services.AddTransient<IAddableCustomerService, CustomerService>();
            //services.AddTransient<IDeletableCustomerService, CustomerService>();
            //services.AddTransient<IUpdateableCustomerService, CustomerService>();
            //services.AddTransient<IDateTimeService, DateTimeService>();

        }

        // Extension Method für alle Repositories
        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IReadOnlyProductRepository, ProductRepository>();
            services.AddTransient<IReadOnlyRepositoryGeneric<ProductCategory>, RepositoryGeneric<ProductCategory>>();


            //services.AddTransient<IReadOnlyRepositoryGeneric<Customer>, RepositoryGeneric<Customer>>();
            //services.AddTransient<IRepositoryGeneric<Customer>, RepositoryGeneric<Customer>>();
        }
    }
}