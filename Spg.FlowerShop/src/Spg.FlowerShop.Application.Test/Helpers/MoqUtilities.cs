using Spg.FlowerShop.Domain.Model;
using Spg.FlowerShop.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spg.FlowerShop.Application.Test.Helpers
{
    public static class MoqUtilities
    {
        public static ProductCategory GetSeedingProductCategory() // expected
        {
            return new ProductCategory(new Guid("4013a0b3-7019-48ea-bb0c-6cc39f9ddedc"), "C1", "Category Description 1");
        }

        public static Product GetSeedingProduct(ProductCategory productCategory) // expected
        {
            return new Product(productCategory.Name + "99", 50, "1234567891234", productCategory, "Url_Image99");
        }

        public static Customer GetSeedingCustomer(Address address) // expected
        {
            return new Customer(new Guid("f41b05f3-e11f-40a2-a0b6-91b56a11c5e4"), Genders.Female, "FirstName Example", "LastName Example",
                "email@example.com", 123456789, new DateTime(2023, 04, 02), new DateTime(1986, 08, 19), 
                new Address("Street Example", "15", "1111", "Vienna"));
        }

        public static Address GetSeedingAddress() 
        { 
            return new Address("Street Example", "15", "1111", "Vienna");
        }
    }
}
