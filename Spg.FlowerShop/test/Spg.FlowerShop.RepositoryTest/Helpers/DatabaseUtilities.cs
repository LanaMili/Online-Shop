using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Spg.FlowerShop.Domain.Model;
using Spg.FlowerShop.Infrastructure;

namespace Spg.FlowerShop.RepositoryTest.Helpers
{
    public static class DatabaseUtilities
    {
        public static DbContextOptions GenerateDbOptions()
        {
            SqliteConnection connection = new SqliteConnection("Data Source = :memory:");
            connection.Open();

            DbContextOptionsBuilder options = new DbContextOptionsBuilder();
            options.UseSqlite(connection);
            return options.Options;
        }

        public static void InitializeDatabase(FlowerShopContext db)
        {
            db.Database.EnsureCreated();

            db.ProductCategories.AddRange(GetSeedingProductCategories());
            db.SaveChanges();

            // Seed Products
            db.Products.AddRange(GetSeedingProducts(db.ProductCategories.Single(pc => pc.Name.Equals("C1"))));

            db.SaveChanges();

            // Seed Customers
            db.Customers.AddRange(GetSeedingCustomers());
            db.SaveChanges();
        }

        private static List<ProductCategory> GetSeedingProductCategories()
        {
            return new List<ProductCategory>()
            {
                new ProductCategory(new Guid("4013a0b3-7019-48ea-bb0c-6cc39f9ddedc"), "C1", "Category Description 1"),
                new ProductCategory(new Guid("5327850e-0118-474e-b85a-a51e3ad02a93"), "C2", "Category Description 2"),
                new ProductCategory(new Guid("eac1fd8b-a097-4f69-a31a-94d6ce969da3"), "C3", "Category Description 3")
            };
        }

        private static List<Product> GetSeedingProducts(ProductCategory productCategory)
        {
            return new List<Product>()
            {
                new Product(productCategory.Name + "99", 50, "1234123412341", productCategory, "Url_Image99"),
            };
        }

        private static List<Customer> GetSeedingCustomers()
        {
            return new List<Customer>()
            {
                new Customer(new Guid("e6a479d1-4963-4f70-8fdc-ea89240d4ac9"), Genders.Female, "CustomerFirstName1", "CustomerLastName1", "email1", 123456, new DateTime(2023, 2, 12))
            };
        }
    }
}
