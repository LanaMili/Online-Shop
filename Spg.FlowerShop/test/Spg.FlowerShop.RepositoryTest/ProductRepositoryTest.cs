using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Spg.FlowerShop.Domain.Exceptions;
using Spg.FlowerShop.Domain.Interfaces;
using Spg.FlowerShop.Domain.Model;
using Spg.FlowerShop.Infrastructure;
using Spg.FlowerShop.Repository;
using Spg.FlowerShop.Repository.Products;
using Spg.FlowerShop.RepositoryTest.Helpers;


namespace Spg.FlowerShop.RepositoryTest
{
    public class ProductRepositoryTest
    {
        [Fact]
        public void Create_Success_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);
                Product entity = new Product("Blumenstrauß", 45, "1234123412341", 
                    db.ProductCategories.Single(pc => pc.Name.Equals("C1")), "Url_Image");

                // Act
                new ProductRepository(db).Create(entity);

                // Assert
                Assert.Equal(2, db.Products.Count());
            }
        }

        [Fact]
        public void Create_ProductRepositoryCreateException_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);

                Product entity = new Product("C499", 50, null!, db.ProductCategories.Single(pc => pc.Name.Equals("C1")), null!);

                // Assert
                Assert.Throws<ProductRepositoryCreateException>(() => new ProductRepository(db).Create(entity));
            }
        }

        [Fact]
        public void Product_GetByPK_Success_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);
                Product expected = new Product("C199", 50, "1234123412341",
                    db.ProductCategories.Single(pc => pc.Name.Equals("C1")), "Url_Image");

                // Act
                Product actual = new RepositoryGeneric<Product>(db).GetByPK<string>("C199");

                // Assert
                Assert.Equal(expected.ProductName, actual.ProductName);
                Assert.Equal(expected.Ean, actual.Ean); 
            }
        }

        [Fact]
        public void Category_GetByPK_Success_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);
                ProductCategory expected = new ProductCategory(new Guid("4013a0b3-7019-48ea-bb0c-6cc39f9ddedc"), "C1", "Category Description 1");

                // Act
                ProductCategory? actual = new RepositoryGeneric<ProductCategory>(db).GetByPK<Guid>(new Guid("4013a0b3-7019-48ea-bb0c-6cc39f9ddedc")); 

                // Assert
                Assert.Equal(expected.Guid, actual?.Guid); 
            }
        }

        [Fact]
        public void Delete_Success_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);
                Product entity = new Product("Blumenstrauß", 45, "1234123412341",
                    db.ProductCategories.Single(pc => pc.Name.Equals("C1")), "Url_Image");

                db.Products.Add(entity);
                db.SaveChanges();

                ProductRepository productRepository = new ProductRepository(db);

                // Act
                productRepository.Delete(entity);

                // Assert
                Product? deletedProduct = db.Products.Find(entity.ProductName);
                Assert.Null(deletedProduct);
            }
        }

        [Fact]
        public void Delete_ProductRepositoryDeleteException_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);

                Product entity = new Product("C499", 50, "1234123412341", db.ProductCategories.Single(pc => pc.Name.Equals("C1")), "Url_Image");

                ProductRepository productRepository = new ProductRepository(db);

                // Act + Assert
                Assert.Throws<ProductRepositoryDeleteException>(() => productRepository.Delete(entity));
            }
        }

        [Fact]
        public void Update_Success_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);
                Product entity = new Product("Blumenstrauß", 45, "1234123412341",
                    db.ProductCategories.Single(pc => pc.Name.Equals("C1")), "Url_Image");

                db.Products.Add(entity);
                db.SaveChanges();

                ProductRepository productRepository = new ProductRepository(db);

                // Modify product - entity Properties
                entity.CurrentPrice = 50;

                // Act
                productRepository.Update(entity);

                // Assert
                Product? updateddProduct = db.Products.Find(entity.ProductName);
                Assert.NotNull(updateddProduct);
                Assert.Equal(50, updateddProduct?.CurrentPrice);
            }
        }

        [Fact]
        public void Update_ProductRepositoryUpdateException_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);

                Product entity = new Product("Blumenstrauß", 65, "1234123412341",
                    db.ProductCategories.Single(pc => pc.Name.Equals("C1")), "Url_Image");

                ProductRepository productRepository = new ProductRepository(db);

                // Assert
                Assert.Throws<ProductRepositoryUpdateException>(() => productRepository.Update(entity));
            }
        }

        [Fact]
        public void Update_ProductRepositoryUpdateNullReferenceException_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);

                ProductRepository productRepository = new ProductRepository(db);

                // Assert
                Assert.Throws<ArgumentException>(() => productRepository.Update(null));
            }
        }
    }
}