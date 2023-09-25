using Bogus;
using Microsoft.EntityFrameworkCore;
using Spg.FlowerShop.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Infrastructure
{
    // 1. FlowerShopContext : DBContext 
    public class FlowerShopContext : DbContext
    {
        // 2. Die Tabellen der DB als Properties auflisten
        public DbSet<Customer> Customers => Set<Customer>(); 
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
        public DbSet<ShoppingCartItem> ShoppingCartItems => Set<ShoppingCartItem>();
        public DbSet<PaymentMethod> PaymentMethods=> Set<PaymentMethod>();
        public DbSet<Price> Prices=> Set<Price>();
        public DbSet<Review> Reviews=> Set<Review>();

        // 3. Constructor
        public FlowerShopContext()
        { }

        public FlowerShopContext(DbContextOptions options)
            : base(options) 
        { 
            Database.EnsureCreated();
        }

        // 4. OnConfuguring() 
        // wenn FlowerShopContext() aufgeruen ist, verwendet dann --> 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    optionsBuilder.UseSqlite("Data Source=FlowerShop.db");
            //}
        }

        // 5. OnModelCreting() 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(p => p.ProductName);
            modelBuilder.Entity<Product>().Property(p => p.ProductName).IsRequired();

            modelBuilder.Entity<ProductCategory>().HasKey(p => p.Guid);
            modelBuilder.Entity<ProductCategory>().Property(p => p.Guid).IsRequired();

            modelBuilder.Entity<PaymentMethod>().HasKey(p => p.Name);
            modelBuilder.Entity<PaymentMethod>().Property(p => p.Name).IsRequired();

            // Value Object
            modelBuilder.Entity<Customer>().OwnsOne(c => c.Address);
            modelBuilder.Entity<ShoppingCart>().OwnsOne(sc => sc.ShippingAddress);
            modelBuilder.Entity<ShoppingCart>().OwnsOne(sc => sc.BillingAddress);
        }

        public void Seed()
        {
            Randomizer.Seed = new Random(1444112);

            // ProductCategory
            string[] descriptions = new string[] { "Box Blumenstrauß", "Frischer Blumenstrauß", "Bunter Blumenstrauß", "Grußkarten" };
            List<ProductCategory> productCategories = new Faker<ProductCategory>("de").CustomInstantiator(f =>
                {
                    var description = f.Random.ArrayElement(descriptions);

                    return new ProductCategory(
                    f.Random.Guid(),
                    description.Substring(0, 6).ToUpper(),
                    description
                    );
                })
                .Generate(10)
                .GroupBy(pc => pc.Name)
                .Select(pc => pc.First()) 
                .ToList();
               
            ProductCategories.AddRange(productCategories);
            SaveChanges();

            // PaymentMethod
            string[] paymentMethodsNames = new string[] { "master", "master/visa", "klarna", "paypal" };
            List<PaymentMethod> paymentMethods = new Faker<PaymentMethod>("de").CustomInstantiator(f =>
                new PaymentMethod(
                    f.Random.ArrayElement(paymentMethodsNames)
                )) 
                .Generate(10)
                .GroupBy(pm => pm.Name)
                .Select(pm => pm.First())
                .ToList();

            PaymentMethods.AddRange(paymentMethods);
            SaveChanges();

            // Customer
            List<Customer> customers = new Faker<Customer>("de").CustomInstantiator(f =>
                new Customer(
                    f.Random.Guid(),
                    f.Random.Enum<Genders>(),
                    f.Name.FirstName(Bogus.DataSets.Name.Gender.Female),
                    f.Name.LastName(),
                    f.Internet.Email(),
                    f.Random.Long(111111, 999999),
                    f.Date.Between(DateTime.Now.AddYears(-5), DateTime.Now.AddDays(-2)),
                    f.Date.Between(DateTime.Now.AddYears(-85), DateTime.Now.AddYears(-16))
                    ))
                .Rules((f, c) =>
                {
                    if (c.Gender == Genders.Male)
                    {
                        c.FirstName = f.Name.FirstName(Bogus.DataSets.Name.Gender.Male);
                    }

                    c.Address = new Address(f.Address.StreetName(), f.Address.BuildingNumber(), f.Address.ZipCode(), f.Address.City());

                c.LastChangeDate = f.Date.Between(new DateTime(2021, 01, 01), DateTime.Now).Date.OrNull(f, 0.2f);

                })
                .Generate(30)
                .ToList();

            Customers.AddRange(customers);
            SaveChanges();


            // ShoppingCart
            List<ShoppingCart> shoppingCarts = new Faker<ShoppingCart>("de").CustomInstantiator(f =>
                new ShoppingCart(
                    f.Random.Guid(),
                    f.Random.Enum<Status>(),
                    new Address(
                        f.Address.StreetName(),
                        f.Address.BuildingNumber(),
                        f.Address.ZipCode(),
                        f.Address.City()
                    ),
                    new Address(
                        f.Address.StreetName(),
                        f.Address.BuildingNumber(),
                        f.Address.ZipCode(),
                        f.Address.City()
                    )
                ))
            .Rules((f, sc) =>
            {
                sc.LastChangeDate = f.Date.Between(new DateTime(2020, 1, 1), DateTime.Now).Date.OrNull(f, 0.3f);
                
                var paymentMethodNavigation = f.Random.ListItem(paymentMethods);
                sc.PaymentMethodNavigation = paymentMethodNavigation;
                sc.PaymentMethodNavigationName = paymentMethodNavigation.Name;

                f.Random.ListItem(customers).AddShopingCart(sc);
            })
            .Generate(10)
            .ToList();
            
            ShoppingCarts.AddRange(shoppingCarts);
            SaveChanges();

            // ProductCategory: "Box Blumenstrauß", "Frischer Blumenstrauß", "Bunter Blumenstrauß", "Grußkarten
            // Product 
            string[] productNames = new string[] { "Blumenstrauß Rosen", "Bouquet Tulpen", "Bunter Blumenstrauß Tulpen", "Bunter Wasserfallstrauß", "Blumenstrauß Amaryllis Blassrosa", "Blumenstrauß Hortensien",
            "Blumenstrauß Lilien Weiß", "Blumenstrauß Nelken", "Box Hortensien", "Box Rosen", "Bunter Blumenstrauß Rosen", "Grußkarte Geburtstag", "Grußkarte Allgemein", "Bunter lumenstrauß Hortensien",
            "Blumenstrauß mit Hortensien und Pfingstrosen", "Sommerstrauß aus Rosen, Löwenmäulchen und Brombeere", "Sommerstrauß aus Rosen, Wicken und Clematis", "Sommerstrauß aus Cosmea, Margeriten und Wicken",
            "Sommerstrauß aus Päonien, Jasmin und Phlox", "Sommerstrauß aus Rosen, Funkien und Wicken", "Sommerstrauß aus Goldgarbe, Rosen und Kängurupfote", "Sommerstrauß aus Levkojen, Rosen und Hortensien",
            "Blumenstrauß mit Amaryllis und Eukalyptus", "Blumenstrauß mit Dahlie und Eukalyptus", "Bunter Blumenstrauß aus Duftwicke, Kornblume und Kugelamarant", "Blumenstrauß Hyazinthen"};

            List<Product> products = new Faker<Product>("de").CustomInstantiator(f => {
                ProductCategory productCategory = f.Random.ListItem(productCategories);
                string productName = GetProductName(f, productCategory, productNames);

                return new Product(
                    productName,
                    f.Random.Decimal(15,150), 
                    f.Commerce.Ean13(),
                    productCategory,
                    f.Image.PicsumUrl()
                );
                })
                .Generate(1000)
                .GroupBy(p => p.ProductName)
                .Select(p => p.First())
                .ToList();
                
            Products.AddRange(products);
            SaveChanges();

            //Review
            List<Review> reviews = new Faker<Review>("de").CustomInstantiator(f => 
                new Review(
                    f.Date.Between(DateTime.Now, DateTime.Now.AddYears(-1)).Date,
                    f.Random.Int(1, 5),
                    f.Lorem.Text(),
                    f.Random.ListItem(products)
                ))
                .Rules((f, r) =>
                {
                    r.LastChangeDate = f.Date.Between(new DateTime(2020, 1, 1), DateTime.Now).Date;
                    f.Random.ListItem(customers).AddReview(r); 
                })
                .Generate(20)
                .ToList();

            Reviews.AddRange(reviews);
            SaveChanges();

            // Price
            //List<Price> prices= new Faker<Price>("de").CustomInstantiator(f => 
            //    new Price(
            //        f.Random.Decimal(10, 150),
            //        f.Random.ListItem(products),
            //        DateTime.Now,
            //        null                        
            //        ))
            //    .Rules( (f, p) =>
            //    {
            //        f.Random.ListItem(products).CurrentPrice = f.Random.Decimal(10, 150);
            //    })
            //    .Generate(10)
            //    .ToList();

            ////Prices.AddRange(prices);
            //SaveChanges();

            // ShoppingCardItem
            List<ShoppingCartItem> items = new Faker<ShoppingCartItem>("de").CustomInstantiator(f =>
                new ShoppingCartItem(
                     f.Random.ListItem(products),
                     f.Random.Int(1, 50)
                ))
                .Rules( (f, sci) =>
                {
                    sci.LastChangeDate = f.Date.Between(new DateTime(2020, 1, 1), DateTime.Now).Date.OrNull(f, 0.3f);
                    sci.Price = f.Random.ListItem(products).CurrentPrice;

                    f.Random.ListItem(shoppingCarts).AddShoppingCartItem(sci);
                })
                .Generate(50)
                .ToList();

            ShoppingCartItems.AddRange(items);
            SaveChanges();
        }

        private static string GetProductName(Faker f, ProductCategory pc, string[] productNames)
        {
            var productName = string.Empty;
            if (pc.Name.ToLower().Contains("box"))
            {
                productName = f.Random.ArrayElement(productNames.Where(pn => pn.StartsWith("Box")).ToArray());
            }
            else if (pc.Name.ToLower().Contains("bunter") || pc.Name.ToLower().Contains("sommerstrauß"))
            {
                productName = f.Random.ArrayElement(productNames.Where(pn => pn.StartsWith("Bunter") || pn.StartsWith("Sommerstrauß")).ToArray());
            }
            else if (pc.Name.ToLower().Contains("grußkarten"))
            {
                productName = f.Random.ArrayElement(productNames.Where(pn => pn.StartsWith("Grußkarte")).ToArray());
            }
            else
            {
                productName = f.Random.ArrayElement(productNames.Where(pn => !pn.StartsWith("Box")
                                                                    && !pn.StartsWith("Bunter")
                                                                    && !pn.StartsWith("Sommerstrauß")
                                                                    && !pn.StartsWith("Grußkarte")).ToArray());
            }

            return productName;
        }
    }
}
