using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Spg.FlowerShop.Application.Products;
using Spg.FlowerShop.Application.Test.Helpers;
using Spg.FlowerShop.Domain.Model;
using Spg.FlowerShop.Infrastructure;
using Spg.FlowerShop.Repository;
using Spg.FlowerShop.Domain.Exceptions;
using Spg.FlowerShop.Repository.Products;
using Spg.FlowerShop.Domain.Interfaces;

namespace Spg.FlowerShop.Application.Test
{
    public class ProductServiceTest
    {
        private ProductService InitUnitToTest(FlowerShopContext db)
        {
            return new ProductService(
                new ProductRepository(db),  
                new ProductRepository(db),
                new RepositoryGeneric<ProductCategory>(db));
        }
        
        private DbContextOptions GenerateDbOptions()
        {
            SqliteConnection connection = new SqliteConnection("Data Source = :memory:");
            connection.Open();

            DbContextOptionsBuilder options = new DbContextOptionsBuilder();
            options.UseSqlite(connection);
            return options.Options;
        }

        [Fact]
        public void CreateProduct_Success_Test()
        { 
            // Arrange
            using (FlowerShopContext db = new FlowerShopContext(GenerateDbOptions()))
            {
                ProductService unitToTest = InitUnitToTest(db);

                DatabaseUtilities.InitializeDatabase(db);

                Product entity = new Product("Test Name Product", 45, "1234123412341",
                    db.ProductCategories.Single(pc => pc.Guid.Equals(new Guid("4013a0b3-7019-48ea-bb0c-6cc39f9ddedc"))), "Url_Image4");

                // Act
                unitToTest.Create(entity);

                // Assert
                Assert.Equal(2, db.Products.ToList().Count());
                Assert.Equal("Test Name Product", db.Products.ToList().ElementAt(1).ProductName);
            }
        }

        [Fact]
        public void CreateProduct_NameNotUnique_CreateProductServiceException_Test()
        {
            // Arrange
            using (FlowerShopContext db = new FlowerShopContext(GenerateDbOptions()))
            {
                ProductService unitToTest = InitUnitToTest(db);

                DatabaseUtilities.InitializeDatabase(db);

                Product entity = new Product("C199", 45, "1234123412341",
                    db.ProductCategories.Single(pc => pc.Name.Equals("C1")), "Url_Image99");

                // Act + Assert
                Assert.Throws<ProductServiceCreateException>(() => unitToTest.Create(entity));
            }
        }
    }
}