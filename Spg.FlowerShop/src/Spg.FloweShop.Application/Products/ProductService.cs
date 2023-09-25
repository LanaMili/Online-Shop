using Spg.FlowerShop.Domain.Exceptions;
using Spg.FlowerShop.Domain.Interfaces;
using Spg.FlowerShop.Domain.Model;
using Spg.FlowerShop.Repository;
using Spg.FlowerShop.Repository.Products;
using System.Text.RegularExpressions;

namespace Spg.FlowerShop.Application.Products
{
    public class ProductService : IReadOnlyProductService, IAddableProductService, IUpdateableProductService, IDeletableProductService
    {
        private readonly IProductRepository _repository;
        private readonly IReadOnlyProductRepository _readOnlyProductRepository;
        private readonly IReadOnlyRepositoryGeneric<ProductCategory> _categoryRepository;

        // Constructor Injection
        public ProductService(
            IProductRepository repository, 
            IReadOnlyProductRepository readOnlyProductRepository,
            IReadOnlyRepositoryGeneric<ProductCategory> categoryRepository
            )
        {
            _repository = repository;
            _readOnlyProductRepository = readOnlyProductRepository;
            _categoryRepository = categoryRepository;
        }

        public IQueryable<Product> GetAll()
        {
            IQueryable<Product> products = _readOnlyProductRepository.GetAll();
            return products;
        }

        public Product? ProductGetById(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                throw new ArgumentException("Der Produktname darf nicht null oder leer sein.");
            }

            Product? product = _readOnlyProductRepository.GetByName(productName);
            return product;
        }

        public void Create(Product newProduct)
        {
            // * No Product Name -> Empty Name 
            // * Not unique Product Name
            // * Es muss eine gültige Kategorie haben
            // * Invalid Ean
            // * Der Preis zwischen 1 und 1000 
            // * Der Dateiname des Produktbildes darf nicht leer sein

            // *** Nur der Admin darf ein Produkt anlegen

            if (string.IsNullOrWhiteSpace(newProduct.ProductName) || _readOnlyProductRepository.GetByName(newProduct.ProductName) is not null)
            {
                throw new ProductServiceCreateException("Produktname ist ungültig oder existiert bereits");
            }
            if (newProduct.ProductCategoryNavigation is null || _categoryRepository.GetByPK<Guid>(newProduct.ProductCategoryNavigation.Guid) is null) 
            {
                throw new ProductServiceCreateException("Produkt Kategorie ist nicht gültig");
            }
            if (string.IsNullOrWhiteSpace(newProduct.Ean) || !Regex.IsMatch(newProduct.Ean, @"^\d{13}$"))
            {
                throw new ProductServiceCreateException("Produkt Ean ist nicht gültig");
            }
            if (!(newProduct.CurrentPrice > 1 && newProduct.CurrentPrice < 1000))
            {
                throw new ProductServiceCreateException("Der Produktpreis muss zwischen 1 und 1000 liegen.");
            }
            if (string.IsNullOrWhiteSpace(newProduct.ProductImage))
            {
                throw new ProductServiceCreateException("Der Dateiname des Produktbildes darf nicht leer sein");
            }

        
          
                _repository.Create(newProduct);
           
        }

        public void Delete(string productName)
        {
            Product? product = _readOnlyProductRepository.GetByName(productName);
            if (product == null)
            {
                throw new ProductServiceDeleteException("Das Produkt existiert nicht!");
            }
            _repository.Delete(product);
        }

        public void Update(Product product)
        {
            if (_readOnlyProductRepository.GetByName(product.ProductName) is null)
            {
                throw new ProductServiceUpdateException("Update nicht möglich, Produkt existiert nicht!");
            }
            if (string.IsNullOrWhiteSpace(product.Ean) || !Regex.IsMatch(product.Ean, @"^\d{13}$"))
            {
                throw new ProductServiceUpdateException("Produkt Ean ist nicht gültig");
            }
            if (!(product.CurrentPrice > 1 && product.CurrentPrice < 1000))
            {
                throw new ProductServiceUpdateException("Current Price ist nicht gültig");
            }
            _repository.Update(product);
        }
    }
}
