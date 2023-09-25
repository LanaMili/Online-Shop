using Spg.FlowerShop.Domain.Model;
using Spg.FlowerShop.Infrastructure;
using Spg.FlowerShop.Repository;
using Spg.FlowerShop.RepositoryTest.Helpers;

namespace Spg.FlowerShop.RepositoryTest
{
    public class CustomerRepositoryTest
    {
        [Fact]
        public void Customer_GetByGuid_Success_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);
                Customer expected = new Customer(
                    new Guid("e6a479d1-4963-4f70-8fdc-ea89240d4ac9"),
                    Genders.Female,
                    "CustomerFirstName1",
                    "CustomerLastName1",
                    "email1", 123456,
                    new DateTime(2023, 2, 12));

                // Act
                Customer? actual = new RepositoryGeneric<Customer>(db).GetByGuid<Customer>(new Guid("e6a479d1-4963-4f70-8fdc-ea89240d4ac9"));

                // Assert
                Assert.Equal(expected.Guid, actual?.Guid);
            }
        }
    }
}