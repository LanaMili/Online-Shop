using Castle.Components.DictionaryAdapter;
using Moq;
using Spg.FlowerShop.Application.Products;
using Spg.FlowerShop.Application.Test.Helpers;
using Spg.FlowerShop.Domain.Exceptions;
using Spg.FlowerShop.Domain.Interfaces;
using Spg.FlowerShop.Domain.Model;
using Spg.FlowerShop.Repository.Products;

namespace Spg.FlowerShop.Application.Test.Moq
{
    public class ProductServiceMoqTest
    {
        private readonly Mock<IReadOnlyProductRepository> _readOnlyProductRepository = new Mock<IReadOnlyProductRepository>();
        private readonly Mock<IProductRepository> _productRepository = new Mock<IProductRepository>();
        private readonly Mock<IReadOnlyRepositoryGeneric<ProductCategory>> _readOnlyProductCategoryRepository = new Mock<IReadOnlyRepositoryGeneric<ProductCategory>>();

        private readonly ProductService _unitToTest;   


        public ProductServiceMoqTest()
        {
            _unitToTest = new ProductService(
                _productRepository.Object,
                _readOnlyProductRepository.Object,
                _readOnlyProductCategoryRepository.Object
                );
        }


        // * GetAll() Method 
        [Fact]
        public void GetAll_Success_Test()
        {
            // Arrange
            List<Product> products = new List<Product>
            {
                new Product("Test Produkt 1", 15, "1234123412341",
                             MoqUtilities.GetSeedingProductCategory(), "Url_Image1"),
                new Product("Test Produkt 2", 25, "2341234123412",
                             MoqUtilities.GetSeedingProductCategory(), "Url_Image2"),
                new Product("Test Produkt 3", 35, "34123412341233",
                             MoqUtilities.GetSeedingProductCategory(), "Url_Image3"),
                new Product("Test Produkt 4", 35, "41234123412334",
                             MoqUtilities.GetSeedingProductCategory(), "Url_Image4")
            };

            _readOnlyProductRepository.Setup(r => r.GetAll()).Returns(products.AsQueryable());

            // Act
            IQueryable<Product>  result = _unitToTest.GetAll();

            // Assert
            Assert.Equal(products.Count, result.Count());
            Assert.True(result.All(p => products.Any(prod => prod.ProductName == p.ProductName 
                                    && prod.CurrentPrice == p.CurrentPrice 
                                    && prod.Ean == p.Ean
                                    && prod.ProductCategoryNavigation == p.ProductCategoryNavigation
                                    && prod.ProductImage == p.ProductImage
                                    )));
        }

        // * ProductGetById(string ProductName) Method 
        [Fact]
        public void GetById_Success_Test()
        {
            // Arrange
            string productName = "Test Product Name";

            Product expectedProduct = new Product(productName, 55, "1234123412341",
                               MoqUtilities.GetSeedingProductCategory(), "Url_Image1");

           _readOnlyProductRepository.Setup(r => r.GetByName(productName))
                .Returns(expectedProduct);

            // Act
            Product? result = _unitToTest.ProductGetById(productName);

            // Assert
            Assert.Equal(expectedProduct, result);
        }

        [Fact]
        public void GetById_ArgumentExceptionNullOrEmptyProductName_Test()
        {
            // Arrange
            string productName = string.Empty;

            // Act + Assert
            Assert.Throws<ArgumentException>(() => _unitToTest.ProductGetById(productName));
        }

        // * Product successfully created
        [Fact]
        public void CreateProduct_Success_Test()
        {
            // Arrange
            Product entity = new Product("Test Produkt 1", 45, "1234123412341",
                                           MoqUtilities.GetSeedingProductCategory(), "Url_Image4");

            _readOnlyProductRepository.Setup(r => r.GetByName("Test Produkt 1"))
                .Returns(null as Product);

            _readOnlyProductCategoryRepository.Setup(pcr => pcr.GetByPK<Guid>(MoqUtilities.GetSeedingProductCategory().Guid))
                .Returns(MoqUtilities.GetSeedingProductCategory());

            _productRepository.Setup(r => r.Create(entity)); // Create aus Repository

            // Act
            _unitToTest.Create(entity);

            // Assert
            _productRepository.Verify(r => r.Create(It.IsAny<Product>()), Times.Once);
        }

        // * No Product Name - Empty Name 
        [Fact]
        public void CreateProduct_EmptyName_ProductServiceCreateException_Test()
        {
            // Arrange
            Product entity = new Product(" ", 45, "1234123412341",
                                           MoqUtilities.GetSeedingProductCategory(), "Url_Image4");

            _readOnlyProductRepository.Setup(r => r.GetByName(" "))
                          .Returns(null as Product);

            _productRepository.Setup(r => r.Create(entity)); // Create aus Repository

            // Act + Assert
            var ex = Assert.Throws<ProductServiceCreateException>(() => _unitToTest.Create(entity));
            Assert.Equal("Produktname ist ungültig oder existiert bereits", ex.Message);
        }

        // * Not unique Product Name
        [Fact]
        public void CreateProduct_NameNotUnique_ProductServiceCreateException_Test()
        {
            // Arrange
            Product entity = new Product("Test Produkt 199", 45, "1234123412341",
                                           MoqUtilities.GetSeedingProductCategory(), "Url_Image4");

            _readOnlyProductRepository.Setup(r => r.GetByName("Test Produkt 199"))
                          .Returns(MoqUtilities.GetSeedingProduct(MoqUtilities.GetSeedingProductCategory()));
            _productRepository.Setup(r => r.Create(entity)); // Create aus Repository

            // Act + Assert
            var ex = Assert.Throws<ProductServiceCreateException>(() => _unitToTest.Create(entity));
            Assert.Equal("Produktname ist ungültig oder existiert bereits", ex.Message);
        }

        // * Es muss eine gültige Kategorie haben
        [Fact]
        public void CreateProduct_InvalidCategory_ProductServiceCreateException_Test()
        {
            // Arrange
            Product entity = new Product("Test Produkt 1", 45, "1234123412341",
                               MoqUtilities.GetSeedingProductCategory(), "Url_Image4");

            _readOnlyProductRepository.Setup(r => r.GetByName("Test Produkt 1"))
                .Returns(null as Product);

            _readOnlyProductCategoryRepository.Setup(pcr => pcr.GetByPK<Guid>(entity.ProductCategoryNavigation.Guid))
                .Returns(null as ProductCategory);

            // Act + Assert
            var ex = Assert.Throws<ProductServiceCreateException>(() => _unitToTest.Create(entity));
            Assert.Equal("Produkt Kategorie ist nicht gültig", ex.Message);
        }

        // * Invalid Ean
        [Fact]
        public void CreateProduct_InvalidEan_ProductServiceCreateException_Test()
        {
            // Arrange
            Product entity = new Product("Test Produkt 1", 45, "invalid Ean",
                               MoqUtilities.GetSeedingProductCategory(), "Url_Image4");

            _readOnlyProductRepository.Setup(r => r.GetByName("Test Produkt 1"))
                .Returns(null as Product);

            _readOnlyProductCategoryRepository.Setup(pcr => pcr.GetByPK<Guid>(MoqUtilities.GetSeedingProductCategory().Guid))
              .Returns(MoqUtilities.GetSeedingProductCategory());

            // Act + Assert
            var ex = Assert.Throws<ProductServiceCreateException>(() => _unitToTest.Create(entity));
            Assert.Equal("Produkt Ean ist nicht gültig", ex.Message);
        }


        // * Der Preis muss zwischen 1 und 1000 sein
        [Fact]
        public void CreateProduct_InvalidPrice_ProductServiceCreateException_Test()
        {
            // Arrange
            Product entity = new Product("Test Produkt 1", -99, "1234123412341",
                   MoqUtilities.GetSeedingProductCategory(), "Url_Image4");

            _readOnlyProductRepository.Setup(r => r.GetByName("Test Produkt 1"))
                .Returns(null as Product);

            _readOnlyProductCategoryRepository.Setup(pcr => pcr.GetByPK<Guid>(MoqUtilities.GetSeedingProductCategory().Guid))
               .Returns(MoqUtilities.GetSeedingProductCategory());

            // Act + Assert
            var ex = Assert.Throws<ProductServiceCreateException>(() => _unitToTest.Create(entity));
            Assert.Equal("Der Produktpreis muss zwischen 1 und 1000 liegen.", ex.Message);
        }

        // * Der Dateiname des Produktbildes darf nicht leer sein
        [Fact]
        public void CreateProduct_WithEmptyUrl_ProductServiceCreateException_Test2()
        {
            // Arrange
            Product entity = new Product("Test Produkt 1", 150, "1234123412341", MoqUtilities.GetSeedingProductCategory(), "");

            _readOnlyProductRepository.Setup(r => r.GetByName("Test Produkt 1"))
                .Returns(null as Product);

            _readOnlyProductCategoryRepository.Setup(pcr => pcr.GetByPK<Guid>(MoqUtilities.GetSeedingProductCategory().Guid))
               .Returns(MoqUtilities.GetSeedingProductCategory());

            // Act + Assert
            var ex = Assert.Throws<ProductServiceCreateException>(() => _unitToTest.Create(entity));
            Assert.Equal("Der Dateiname des Produktbildes darf nicht leer sein", ex.Message);
        }

        // * Product successfully delated
        [Fact]
        public void DeleteProduct_Success_Test()
        {
            // Arrange
            Product? productToDelete = MoqUtilities.GetSeedingProduct(MoqUtilities.GetSeedingProductCategory());

            _readOnlyProductRepository.Setup(r => r.GetByName("Any name"))
                .Returns(productToDelete);

            _productRepository.Setup(r => r.Delete(productToDelete));

            // Act
            _unitToTest.Delete("Any name");

            // Assert
            _productRepository.Verify(r => r.Delete(productToDelete), Times.Once);
        }


        [Fact]
        public void DeleteProduct_ProductRepositoryDeleteException_Test()
        {
            // Arrange
            _readOnlyProductRepository.Setup(r => r.GetByName("No name"))
                .Returns(null as Product);

            // Act + Assert
            var ex = Assert.Throws<ProductServiceDeleteException>(() => _unitToTest.Delete("No name"));
            Assert.Equal("Das Produkt existiert nicht!", ex.Message);
        }


        // * Product successfully updated
        [Fact]
        public void UpdateProduct_Success_Test()
        {
            // Arrange
            Product updatedProduct = new Product("Test Produkt 1", 45, "1234123412341",
                               MoqUtilities.GetSeedingProductCategory(), "Url_Image4");

            _readOnlyProductRepository.Setup(r => r.GetByName("Test Produkt 1"))
                .Returns(updatedProduct);

            _productRepository.Setup(r => r.Update(updatedProduct));

            // Act
            _unitToTest.Update(updatedProduct);

            // Assert
            _productRepository.Verify(r => r.Update(updatedProduct), Times.Once);
        }

        [Fact]
        public void UpdateProduct_ProductServiceUpdateException_Test()
        {
            // Arrange
            Product updatedProduct = new Product("Test Produkt 1", 45, "1234123412341",
                   MoqUtilities.GetSeedingProductCategory(), "Url_Image4");

            _readOnlyProductRepository.Setup(r => r.GetByName("Test Produkt 1"))
                .Returns(null as Product);

            _productRepository.Setup(r => r.Update(updatedProduct));

            // Act + Assert
            var ex = Assert.Throws<ProductServiceUpdateException>(() => _unitToTest.Update(updatedProduct));
            Assert.Equal("Update nicht möglich, Produkt existiert nicht!", ex.Message);
        }

        [Fact]
        public void UpdateProduct_InvalidEan_ProductServiceUpdateException_Test()
        {
            // Arrange
            Product updatedProduct = new Product("Test Produkt 1", 45, "invalid Ean",
                   MoqUtilities.GetSeedingProductCategory(), "Url_Image4");

            _readOnlyProductRepository.Setup(r => r.GetByName("Test Produkt 1"))
                .Returns(MoqUtilities.GetSeedingProduct(MoqUtilities.GetSeedingProductCategory()));

            _readOnlyProductCategoryRepository.Setup(pcr => pcr.GetByPK<Guid>(MoqUtilities.GetSeedingProductCategory().Guid))
              .Returns(MoqUtilities.GetSeedingProductCategory());

            // Act + Assert
            var ex = Assert.Throws<ProductServiceUpdateException>(() => _unitToTest.Update(updatedProduct));
            Assert.Equal("Produkt Ean ist nicht gültig", ex.Message);
        }

        [Fact]
        public void UpdateProduct_InvalidCCurrentPrice_ProductServiceUpdateException_Test()
        {
            // Arrange
            Product updatedProduct = new Product("Test Produkt 1", -101, "1234123412341",
                   MoqUtilities.GetSeedingProductCategory(), "Url_Image4");

            _readOnlyProductRepository.Setup(r => r.GetByName("Test Produkt 1"))
                .Returns(MoqUtilities.GetSeedingProduct(MoqUtilities.GetSeedingProductCategory()));

            _readOnlyProductCategoryRepository.Setup(pcr => pcr.GetByPK<Guid>(MoqUtilities.GetSeedingProductCategory().Guid))
              .Returns(MoqUtilities.GetSeedingProductCategory());

            // Act + Assert
            var ex = Assert.Throws<ProductServiceUpdateException>(() => _unitToTest.Update(updatedProduct));
            Assert.Equal("Current Price ist nicht gültig", ex.Message);
        }
    }
}
