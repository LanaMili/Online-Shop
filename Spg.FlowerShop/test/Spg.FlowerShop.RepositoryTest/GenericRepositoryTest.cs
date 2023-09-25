using Spg.FlowerShop.Domain.Exceptions;
using Spg.FlowerShop.Domain.Interfaces;
using Spg.FlowerShop.Domain.Model;
using Spg.FlowerShop.Infrastructure;
using Spg.FlowerShop.Repository;
using Spg.FlowerShop.Repository.Products;
using Spg.FlowerShop.RepositoryTest.Helpers;

namespace Spg.FlowerShop.RepositoryTest
{
    public class GenericRepositoryTest
    {
        [Fact]
        public void Customer_GetByPK_Success_Test()
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
                    "email1",
                    123456,
                    new DateTime(2023, 2, 12));

                db.Customers.Add(expected);
                db.SaveChanges();

                // Act
                Customer? actual = new RepositoryGeneric<Customer>(db).GetByPK<int>(expected.Id);

                // Assert
                Assert.Equal(expected.Id, actual?.Id);
            }
        }

        [Fact]
        public void Customer_GetByMail_Success_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);
                Customer expected = new Customer(
                    new Guid("a4a478d1-4963-4f70-8fdc-ea89240d4ac9"),
                    Genders.Female,
                    "CustomerFirstName1",
                    "CustomerLastName1",
                    "email2",
                    123456,
                    new DateTime(2023, 2, 12));

                db.Customers.Add(expected);
                db.SaveChanges();

                // Act
                Customer? actual = new RepositoryGeneric<Customer>(db).GetByMail<Customer>(expected.Email);

                // Assert
                Assert.Equal(expected.Email, actual?.Email);
            }
        }

        [Fact]
        public void Customer_GetByGuid_Success_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);
                Customer expected = new Customer(
                    new Guid("a4a478d1-4963-4f70-8fdc-ea89240d4ac9"),
                    Genders.Female,
                    "CustomerFirstName1",
                    "CustomerLastName1",
                    "email1", 
                    123456,
                    new DateTime(2023, 2, 12));

                db.Customers.Add(expected);
                db.SaveChanges();

                // Act
                Customer? actual = new RepositoryGeneric<Customer>(db).GetByGuid<Customer>(expected.Guid);

                // Assert
                Assert.Equal(expected.Guid, actual?.Guid);
            }
        }

        [Fact]
        public void CreateNewCustomer_Success_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);
                Customer newEntity = new Customer(
                    new Guid("e6a479d1-4963-4f70-8fdc-ea89240d4ac9"),
                    Genders.Female,
                    "CustomerFirstName1",
                    "CustomerLastName1",
                    "email1",
                    123456,
                    new DateTime(2023, 2, 12));

                // Act
                new RepositoryGeneric<Customer>(db).Create(newEntity);

                // Assert
                Assert.Equal(2, db.Customers.Count());
            }
        }


        [Fact]
        public void Create_GenericRepositoryCreateException_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);

                Customer newEntity = new Customer(
                    new Guid("e6a479d1-4963-4f70-8fdc-ea89240d4ac9"),
                    Genders.Female,
                    null!,
                    null!,
                    null!, 
                    123456,
                    new DateTime(2023, 2, 12));

                // Assert
                Assert.Throws<GenericRepositoryCreateException>(() => new RepositoryGeneric<Customer>(db).Create(newEntity));
            }
        }


        [Fact]
        public void Delete_Success_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);
                Customer entityToBeDeleted = new Customer(
                    new Guid("e6a479d1-4963-4f70-8fdc-ea89240d4ac9"),
                    Genders.Female,
                    "CustomerFirstName1",
                    "CustomerLastName1",
                    "email1",
                    123456,
                    new DateTime(2023, 2, 12));

                db.Customers.Add(entityToBeDeleted);
                db.SaveChanges();

                RepositoryGeneric<Customer> customerRepository = new RepositoryGeneric<Customer>(db);

                // Act
                customerRepository.Delete(entityToBeDeleted);

                // Assert
                Customer? deletedProduct = db.Customers.Find(entityToBeDeleted.Id);
                Assert.Null(deletedProduct);
            }
        }

        [Fact]
        public void Delete_CustomerServiceCreateException_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);

                Customer entityToBeDeleted = new Customer(
                    new Guid("e6a479d1-4963-4f70-8fdc-ea89240d4ac9"),
                    Genders.Female,
                    "CustomerFirstName1",
                    "CustomerLastName1",
                    "email1",
                    123456,
                    new DateTime(2023, 2, 12));

                db.Customers.Add(entityToBeDeleted);
                db.SaveChanges();

                RepositoryGeneric<Customer> customerRepository = new RepositoryGeneric<Customer>(db);
                customerRepository.Delete(entityToBeDeleted);

                // Act + Assert
                Assert.Throws<GenericRepositoryDeleteException>(() => customerRepository.Delete(entityToBeDeleted));
            }
        }

        [Fact]
        public void Update_Success_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);
                Customer entityToBeUpdated= new Customer(
                    new Guid("e6a479d1-4963-4f70-8fdc-ea89240d4ac9"),
                    Genders.Female,
                    "CustomerFirstName1",
                    "CustomerLastName1",
                    "email1",
                    123456,
                    new DateTime(2023, 2, 12));

                db.Customers.Add(entityToBeUpdated);
                db.SaveChanges();

                RepositoryGeneric<Customer> customerRepository = new RepositoryGeneric<Customer>(db);

                entityToBeUpdated.LastName = "NewLastName";

                // Act
                customerRepository.Update(entityToBeUpdated);

                // Assert
                Customer? updatedCustomer = db.Customers.Find(entityToBeUpdated.Id);
                Assert.NotNull(updatedCustomer);
                Assert.Equal(updatedCustomer.LastName, entityToBeUpdated?.LastName);
            }
        }

        [Fact]
        public void Update_GenericRepositoryUpdateNullReferenceException_Test()
        {
            // Arange (Entity, DB)
            using (FlowerShopContext db = new FlowerShopContext(DatabaseUtilities.GenerateDbOptions()))
            {
                DatabaseUtilities.InitializeDatabase(db);

                RepositoryGeneric<Customer> customerRepository = new RepositoryGeneric<Customer>(db);

                // Assert
                Assert.Throws<ArgumentException>(() => customerRepository.Update(default!));
            }
        }

    }
}