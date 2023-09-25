using Microsoft.EntityFrameworkCore;
using Spg.FlowerShop.Application.Products;
using Spg.FlowerShop.Domain.Model;
using Spg.FlowerShop.Infrastructure;
using Spg.FlowerShop.Repository;
using Spg.FlowerShop.Repository.Products;

namespace Spg.FlowerShop.ConsoleFrontEnd
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite("Data Source=FlowerShop.db");

            FlowerShopContext db = new FlowerShopContext(optionsBuilder.Options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Seed();


            DbContextOptionsBuilder optionsBuilder2 = new DbContextOptionsBuilder();
            optionsBuilder.UseSqlite("Data Source=FlowerShop_Test.db");

            FlowerShopContext db2 = new FlowerShopContext(optionsBuilder.Options);
            db2.Database.EnsureDeleted();
            db2.Database.EnsureCreated();
            db2.Seed();

            // Service aufrufen
            IQueryable<Product> result = new ProductService(new ProductRepository(db), new ProductRepository(db), new RepositoryGeneric<ProductCategory>(db)).GetAll(); 

            foreach (Product p in result.ToList())
            {
                Console.WriteLine($"{ p.ProductName} - {p.Ean} - {p.ProductImage}");
            }
        }
    }
}