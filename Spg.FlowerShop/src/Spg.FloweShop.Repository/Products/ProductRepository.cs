using Microsoft.EntityFrameworkCore;
using Spg.FlowerShop.Domain.Exceptions;
using Spg.FlowerShop.Domain.Interfaces;
using Spg.FlowerShop.Domain.Model;
using Spg.FlowerShop.Infrastructure;

namespace Spg.FlowerShop.Repository.Products
{
    public class ProductRepository : IProductRepository, IReadOnlyProductRepository
    {
        // ProductRepository verwendet DB, die ProductRepository bekommt
        private readonly FlowerShopContext _db;

        public ProductRepository(FlowerShopContext db)
        {
            _db = db;
        }

        public Product? GetByName(string produktName)
        {
            return _db.Products
                .Include(p => p.ProductCategoryNavigation)
                .Include(p => p.Reviews)
                .SingleOrDefault(e => e.ProductName == produktName);
        }

        public IQueryable<Product> GetAll()
        {
            return _db.Set<Product>();
        }

        public void Create(Product newProduct)
        {
            try
            {
                DbSet<Product> dbSet = _db.Set<Product>();
                dbSet.Add(newProduct);
                _db.SaveChanges();  // => Insert
            }
            catch(NullReferenceException ex)
            {
                throw new ProductRepositoryCreateException("Da Produkt existiert nicht, create ist nicht moeglich", ex);
            }
            catch (DbUpdateException ex)
            {
                throw new ProductRepositoryCreateException("Create nicht möglich", ex);
            }
        }

        public void Delete(Product exEntity)
        {
            if (exEntity == null)
            {
                throw new ArgumentException(nameof(exEntity));
            }

            try
            {
                DbSet<Product> dbSet = _db.Set<Product>();
                dbSet.Remove(exEntity);
                _db.SaveChanges();  // => Deleted
            }
            catch (DbUpdateException ex)
            {
                throw new ProductRepositoryDeleteException("Delete ist nicht moeglich", ex);
            }
        }
        public void Update(Product product)
        {
            if (product == null)
            {
                throw new ArgumentException(nameof(product));
            }

            try
            {
                DbSet<Product> dbSet = _db.Set<Product>();
                dbSet.Update(product);
                _db.SaveChanges();  // => Update
            }
            catch (DbUpdateException ex)
            {
                throw new ProductRepositoryUpdateException("Update nicht möglich", ex);
            }
        }
    }
}
