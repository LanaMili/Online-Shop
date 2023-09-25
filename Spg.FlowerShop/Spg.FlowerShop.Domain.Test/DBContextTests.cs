using Microsoft.EntityFrameworkCore;
using Spg.FlowerShop.Domain.Model;
using Spg.FlowerShop.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Domain.Test
{
    public class DBContextTests
    {
        private FlowerShopContext GenerateDb()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite("Data Source=FlowerShop_Test.db");

            FlowerShopContext db = new FlowerShopContext(optionsBuilder.Options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            return db;
        }

        [Fact]
        public void SeedDb()
        {
            FlowerShopContext db = GenerateDb();
            db.Seed();
            Assert.True(true);
        }

        [Fact]
        public void ProductCategory_Add_OneEntity_SuccessTest()
        {
            FlowerShopContext db = GenerateDb();
            ProductCategory newProductCategory = new(new Guid("b6120f2a-278f-4962-9c42-0a973a7fcce1"), "Bouquet Tulpen", "FRISCH");

            db.ProductCategories.Add(newProductCategory);
            db.SaveChanges();

            int actual = db.ProductCategories.Count();
            Assert.Equal(1, actual);
        }

        [Fact]
        public void PaymentMethod_Add_OneEntity_SuccessTest()
        {
            FlowerShopContext db = GenerateDb();
            PaymentMethod newPaymentMethod = new("master");

            db.PaymentMethods.Add(newPaymentMethod);
            db.SaveChanges();

            int actual = db.PaymentMethods.Count();
            Assert.Equal(1, actual);
        }

        [Fact]
        public void Customer_Add_OneEntity_SuccessTest()
        {
            FlowerShopContext db = GenerateDb();
            Customer newCustomer = new(
                new Guid("1d4adee7-a6dc-4f50-a8be-82ae9edee458"),
                Genders.Female,
                "Elena",
                "Fox",
                "efox@gmail.com",
                123456789,
                DateTime.UtcNow.AddMonths(-8),
                DateTime.UtcNow.AddMonths(-25),
                new Address("Street1", "15", "1389", "Vienna")
                );
            db.Customers.Add(newCustomer);
            db.SaveChanges();

            int actual = db.Customers.Count();
            Assert.Equal(1, actual);
        }


        [Fact]
        public void ShoppingCart_Add_OneEntity_SuccessTest()
        {
            FlowerShopContext db = GenerateDb();
            Customer c = new(
                new Guid("92ef820b-6579-420d-86e7-2c8c36a5a6fc"),
                Genders.Female,
                "Elena",
                "Fox",
                "efox@gmail.com",
                123456789,
                DateTime.UtcNow.AddMonths(-8),
                DateTime.UtcNow.AddMonths(-25),
                new Address("Street1", "15", "1389", "Vienna")
                );
            ShoppingCart newShoppingCart = new(
                new Guid("3a265689-9f6b-465e-ab5f-9fca74e19621"),
                Status.Aktuell,
                new Address("Street2", "12", "2345", "Vienna"),
                new Address("Street2", "12", "2345", "Vienna")
                )
            {
                PaymentMethodNavigation = new PaymentMethod("master")
            };

            db.Customers.Add(c);
            c.AddShopingCart(newShoppingCart);
            db.SaveChanges();

            int actual = db.ShoppingCarts.Count();
            Assert.Equal(1, actual);
        }



        [Fact]
        public void Product_Add_OneEntity_SuccessTest()
        {
            FlowerShopContext db = GenerateDb();
            Product newProduct = new(
                "productName",
                30,
                "productEan",
                new ProductCategory(new Guid("b6120f2a-278f-4962-9c42-0a973a7fcce1"), "productName", "description"),
                "productImage"
                );

            db.Products.Add(newProduct);
            db.SaveChanges();

            int actual = db.Products.Count();
            Assert.Equal(1, actual);
        }


        [Fact]
        public void Review_Add_OneEntity_SuccessTest()
        {
            FlowerShopContext db = GenerateDb();
            Customer c = new(
                new Guid("92ef820b-6579-420d-86e7-2c8c36a5a6fc"),
                Genders.Female,
                "Elena",
                "Fox",
                "efox@gmail.com",
                123456789,
                DateTime.UtcNow.AddMonths(-8),
                DateTime.UtcNow.AddMonths(-25),
                new Address("Street1", "15", "1389", "Vienna")
                );
            Review newReview = new(
                DateTime.UtcNow.AddMonths(-2),
                1,
                "alles super",
                new Product("name", 50, "ean", new ProductCategory(new Guid("a7a1ac74-a55e-4d87-81a1-266418b7df20"), "name1", "description1"), "image")
                );

            db.Customers.Add(c);
            c.AddReview(newReview);
            db.SaveChanges();
            int actual = db.Reviews.Count();
            Assert.Equal(1, actual);
        }


        // Setting Current price fills Price table
        [Fact]
        public void Price_Add_OneEntity_SuccessTest()
        {
            FlowerShopContext db = GenerateDb();
            Product p = new Product("name", 45, "ean", new ProductCategory(new Guid("a7a1ac74-a55e-4d87-81a1-266418b7df20"), "name1", "description1"), "image"); 

            db.Products.Add(p); // 
            db.SaveChanges();
            int actual = db.Prices.Count();
            Assert.Equal(1, actual);
        }

        // Setting Current price fills Price table
        [Fact]
        public void CurrentPrice_Set_SuccessTest()
        {
            FlowerShopContext db = GenerateDb();
            Product p = new Product("name", 60, "ean", new ProductCategory(new Guid("a7a1ac74-a55e-4d87-81a1-266418b7df20"), "name1", "description1"), "image");
            p.CurrentPrice = 80;

            db.Products.Add(p); // 
            db.SaveChanges();
            int actual = db.Prices.Count();
            Assert.Equal(2, actual);
            Assert.Equal(80, p.CurrentPrice);
        }

        [Fact]
        public void ShoppingCartItem_Add_OneEntity_SuccessTest()
        {
            FlowerShopContext db = GenerateDb();
            Customer c = new(
                new Guid("92ef820b-6579-420d-86e7-2c8c36a5a6fc"),
                Genders.Female,
                "Elena",
                "Fox",
                "efox@gmail.com",
                123456789,
                DateTime.UtcNow.AddMonths(-8),
                DateTime.UtcNow.AddMonths(-25),
                new Address("Street1", "15", "1389", "Vienna")
                );

            ShoppingCart sc = new(
                new Guid("3a265689-9f6b-465e-ab5f-9fca74e19621"),
                Status.Aktuell,
                new Address("Street2", "12", "2345", "Vienna"),
                new Address("Street2", "12", "2345", "Vienna")
                )
            {
                PaymentMethodNavigation = new PaymentMethod("master")
            };

            db.Customers.Add(c);
            c.AddShopingCart(sc);

            ShoppingCartItem newShoppigCardItem = new(
                new Product("name", 50, "ean", new ProductCategory(new Guid("a7a1ac74-a55e-4d87-81a1-266418b7df20"), "name1", "description1"), "image"),
                2
                );

            db.ShoppingCarts.Add(sc);
            sc.AddShoppingCartItem(newShoppigCardItem);
            db.SaveChanges();

            int actual = db.ShoppingCartItems.Count();
            Assert.Equal(1, actual);
        }
    }
}
