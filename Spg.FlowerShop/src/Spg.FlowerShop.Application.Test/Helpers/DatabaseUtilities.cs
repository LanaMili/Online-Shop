using Spg.FlowerShop.Domain.Model;
using Spg.FlowerShop.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Application.Test.Helpers
{
    public static class DatabaseUtilities
    {
        public static void InitializeDatabase(FlowerShopContext db)
        { 
            // db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            db.ProductCategories.AddRange(GetSeedingProductCategories());
            db.SaveChanges();

            // Seed Products
            db.Products.AddRange(GetSeedingProducts(db.ProductCategories.Single(pc => pc.Name.Equals("C1"))));
            //db.Products.AddRange(GetSeedingProducts(db.ProductCategories.Single(pc => pc.Name.Equals("Category 2"))));
            //db.Products.AddRange(GetSeedingProducts(db.ProductCategories.Single(pc => pc.Name.Equals("Category 3"))));

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
                new Product(productCategory.Name + "99", 50, "1234567891234", productCategory, "Url_Image99"),
                //new Product(productCategory.Name + "002", 40, "234567", productCategory, "Url_Image2"),
                //new Product(productCategory.Name + "003", 35, "345678", productCategory, "Url_Image3"),
                //new Product(productCategory.Name + "004", 60, "456789", productCategory, "Url_Image4"),
                //new Product(productCategory.Name + "005", 45, "567891", productCategory, "Url_Image5"),
            };
        }
    }
}
