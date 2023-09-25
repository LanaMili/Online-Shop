using Moq;
using Spg.FlowerShop.Application.Customers;
using Spg.FlowerShop.Application.Test.Helpers;
using Spg.FlowerShop.Domain.Exceptions;
using Spg.FlowerShop.Domain.Interfaces;
using Spg.FlowerShop.Domain.Model;
using Spg.FlowerShop.Repository.Products;

namespace Spg.FlowerShop.Application.Test.Moq
{
    public class CustomerServiceMoqTest
    {
        private readonly Mock<IReadOnlyRepositoryGeneric<Customer>> _readOnlyCustomersRepository = new Mock<IReadOnlyRepositoryGeneric<Customer>>();
        private readonly Mock<IRepositoryGeneric<Customer>> _customerRepository = new Mock<IRepositoryGeneric<Customer>>();
        private readonly Mock<IDateTimeService> _dateTimeService = new Mock<IDateTimeService>();

        private readonly CustomerService _unitToTestCustomerServiceCreate;  

        public CustomerServiceMoqTest()
        {
            _unitToTestCustomerServiceCreate = new CustomerService(
                _readOnlyCustomersRepository.Object,
                _customerRepository.Object,
                _dateTimeService.Object
            );
        }

        // * 
        [Fact]
        public void CreateCustomer_DefaultNewCustomer_CustomerServiceCreateException_Test()
        {
            // Arrange + Act + Assert
            var ex = Assert.Throws<CustomerServiceCreateException>(() => _unitToTestCustomerServiceCreate.Create(default!));
            Assert.Equal("Fehlender Kunde", ex.Message);
        }

        // * Create(entity) from CustomerService successfully calls Create(entity) from Repository
        [Fact]
        public void CreateCustomer_Success_Test()
        {
            // Arrange
            Customer entity = new Customer(new Guid("f41b05f3-e11f-40a2-a0b6-91b56a11c5e4"), Genders.Female, "FirstName Example",
                "LastName Example", "email@example.com", 123456789, new DateTime(2023, 04, 02), new DateTime(1986, 08, 19),
                new Address("Street Example", "15", "1111", "Vienna"));

            _dateTimeService.Setup(dt => dt.Now).Returns(new DateTime(2023, 05, 03));

            // Act
            _unitToTestCustomerServiceCreate.Create(entity);

            // Assert
            _customerRepository.Verify(r => r.Create(It.IsAny<Customer>()), Times.Once);
        }

        // * First Name Empty; max 32 letters
        [Fact]
        public void CreateCustomer_EmptyName_CustomerServiceCreateException_Test()
        {
            // Arrange
            Customer entity = new Customer(new Guid("f41b05f3-e11f-40a2-a0b6-91b56a11c5e4"), Genders.Female, "",
                "LastName Example", "email@example.com", 123456789, new DateTime(2023, 04, 02), new DateTime(1986, 08, 19),
                new Address("Street Example", "15", "1111", "Vienna"));


            // Act + Assert
            var ex = Assert.Throws<CustomerServiceCreateException>(() => _unitToTestCustomerServiceCreate.Create(entity));
            Assert.Equal("Vorname ist ungültig", ex.Message);
        }


        // * Last Name empty; max 32 letters
        [Fact]
        public void CreateCustomer_EmptyLastName_CustomerServiceCreateException_Test()
        {
            // Arrange
            Customer entity = new Customer(new Guid("f41b05f3-e11f-40a2-a0b6-91b56a11c5e4"), Genders.Female, "FirstName Example",
                "", "email@example.com", 123456789, new DateTime(2023, 04, 02), new DateTime(1986, 08, 19),
                new Address("Street Example", "15", "1111", "Vienna"));


            // Act + Assert
            var ex = Assert.Throws<CustomerServiceCreateException>(() => _unitToTestCustomerServiceCreate.Create(entity));
            Assert.Equal("Nachname ist ungültig", ex.Message);
        }

        // * Email empty
        [Fact]
        public void CreateCustomer_InvalidEmail_CustomerServiceCreateException_Test()
        {
            // Arrange
            Customer entity = new Customer(new Guid("f41b05f3-e11f-40a2-a0b6-91b56a11c5e4"), Genders.Female, "FirstName Example",
                "LastName Example", "", 123456789, new DateTime(2023, 04, 02), new DateTime(1986, 08, 19),
                new Address("Street Example", "15", "1111", "Vienna"));

            // Act + Assert
            var ex = Assert.Throws<CustomerServiceCreateException>(() => _unitToTestCustomerServiceCreate.Create(entity));
            Assert.Equal("Email ist ungültig", ex.Message);
        }

        // * Email without @
        [Fact]
        public void CreateCustomer_InvalidEmailNoProperFormat_CustomerServiceCreateException_Test()
        {
            // Arrange
            Customer entity = new Customer(new Guid("f41b05f3-e11f-40a2-a0b6-91b56a11c5e4"), Genders.Female, "FirstName Example",
                "LastName Example", "email.com", 123456789, new DateTime(2023, 04, 02), new DateTime(1986, 08, 19),
                new Address("Street Example", "15", "1111", "Vienna"));

            // Act + Assert
            var ex = Assert.Throws<CustomerServiceCreateException>(() => _unitToTestCustomerServiceCreate.Create(entity));
            Assert.Equal("Email ist ungültig", ex.Message);
        }

        // * max 32 letters; 
        [Fact]
        public void CreateCustomer_InvalidEmailMoreThan32Characters_CustomerServiceCreateException_Test()
        {
            // Arrange
            Customer entity = new Customer(new Guid("f41b05f3-e11f-40a2-a0b6-91b56a11c5e4"), Genders.Female, "FirstName Example",
                "LastName Example", "InvalidEmailMoreThan32CharactersInvalidEmalMoreThan32Characters@example.com", 123456789,
                new DateTime(2023, 04, 02), new DateTime(1986, 08, 19), new Address("Street Example", "15", "1111", "Vienna"));

            // Act + Assert
            var ex = Assert.Throws<CustomerServiceCreateException>(() => _unitToTestCustomerServiceCreate.Create(entity));
            Assert.Equal("Email ist ungültig", ex.Message);
        }

        // * No Gender set / Unknown gender
        [Fact]
        public void CreateCustomer_InvalidGender_CustomerServiceCreateException_Test()
        {
            // Arrange
            Customer entity = new Customer(new Guid("f41b05f3-e11f-40a2-a0b6-91b56a11c5e4"), (Genders)86, "FirstName Example",
                "LastName Example", "email@example.com", 123456789,
                new DateTime(2023, 04, 02), new DateTime(1986, 08, 19), new Address("Street Example", "15", "1111", "Vienna"));

            // Act + Assert
            var ex = Assert.Throws<CustomerServiceCreateException>(() => _unitToTestCustomerServiceCreate.Create(entity));
            Assert.Equal("Geschlecht ist ungültig", ex.Message);
        }

        // * default RegistrationDateTime value Moq
        [Fact]
        public void CreateCustomer_DefaultRegistrationDateTime_CustomerServiceCreateException_Test()
        {
            // Arrange
            Customer entity = new Customer(new Guid("f41b05f3-e11f-40a2-a0b6-91b56a11c5e4"), Genders.Female, "FirstName Example",
                "LastName Example", "email@example.com", 123456789, DateTime.MinValue, new DateTime(1986, 08, 19),
                new Address("Street Example", "15", "1111", "Vienna"));

            // Act + Assert
            var ex = Assert.Throws<CustomerServiceCreateException>(() => _unitToTestCustomerServiceCreate.Create(entity));
            Assert.Equal("Registrierungsdatum hat Defaultwert und ist ungültig", ex.Message);
        }

        // * Invalid RegistrationDateTime value 
        [Fact]
        public void CreateCustomer_InvalidRegistrationDateTime_CustomerServiceCreateException_Test()
        {
            // Arrange
            Customer entity = new Customer(new Guid("f41b05f3-e11f-40a2-a0b6-91b56a11c5e4"), Genders.Female, "FirstName Example",
                "LastName Example", "email@example.com", 123456789, new DateTime(2023, 04, 02), new DateTime(1986, 08, 19),
                new Address("Street Example", "15", "1111", "Vienna"));

            _dateTimeService.Setup(dt => dt.Now).Returns(new DateTime(2022, 04, 01));

            // Act + Assert
            var ex = Assert.Throws<CustomerServiceCreateException>(() => _unitToTestCustomerServiceCreate.Create(entity));
            Assert.Equal("Registration Date ist ungültig, das Registrierungsdatum muss in der Vergangenheit liegen", ex.Message);
        }

        //// * invalid Address
        //[Fact]
        //public void CreateCustomer_InvalidAddress_CustomerServiceCreateException_Test()
        //{
        //    // Arrange
        //    Customer entity = new Customer(new Guid("f41b05f3-e11f-40a2-a0b6-91b56a11c5e4"), Genders.Female, "FirstName Example",
        //        "LastName Example", "email@example.com", 123456789, new DateTime(2023, 04, 02), new DateTime(1986, 08, 19),
        //        new Address(string.Empty, "15", "1111", string.Empty));

        //    _dateTimeService.Setup(dt => dt.Now).Returns(new DateTime(2023, 05, 03));

        //    // Act + Assert
        //    var ex = Assert.Throws<CustomerServiceCreateException>(() => _unitToTestCustomerServiceCreate.Create(entity));
        //    Assert.Equal("Die Adrese ist ungültig", ex.Message);
        //}

        // * Customer successfully delated
        [Fact]
        public void DeleteCustomer_Success_Test()
        {
            // Arrange
            Customer? customerToBeDeleted = MoqUtilities.GetSeedingCustomer(MoqUtilities.GetSeedingAddress());

            _readOnlyCustomersRepository.Setup(r => r.GetByGuid<Customer>(new Guid("f41b05f3-e11f-40a2-a0b6-91b56a11c5e4")))
                .Returns(customerToBeDeleted);

            _customerRepository.Setup(r => r.Delete(customerToBeDeleted));

            // Act
            _unitToTestCustomerServiceCreate.Delete(new Guid("f41b05f3-e11f-40a2-a0b6-91b56a11c5e4"));

            // Assert
            _customerRepository.Verify(r => r.Delete(customerToBeDeleted), Times.Once);
        }

        // * CustomerServiceDeleteException successfully thrown
        [Fact]
        public void DeleteCustomer_CustomerServiceDeleteException_Test()
        {
            // Arrange
            _readOnlyCustomersRepository.Setup(r => r.GetByGuid<Customer>(new Guid("f41b05f3-e11f-40a2-a0b6-91b56a11c5e4")))
                .Returns(null as Customer);

            // Act + Assert
            var ex = Assert.Throws<CustomerServiceDeleteException>(() => _unitToTestCustomerServiceCreate.Delete(new Guid("f41b05f3-e11f-40a2-a0b6-91b56a11c5e4")));
            Assert.Equal("Der Kunde existiert nicht!", ex.Message);
        }

        // * Customer successfully updated
        [Fact]
        public void UpdateCustomer_Success_Test()
        {
            // Arrange
            Customer? customerToBeUpdated = new Customer(
                new Guid("a41b05f3-e11f-40a2-a0b6-91b56a11c5e4"), 
                Genders.Female, 
                "Cust First Name", 
                "Cust Last Name", 
                "cust@example.mail",
                123456,
                new DateTime(2023, 04, 02), 
                new DateTime(1986, 08, 19), 
                new Address("Street Example2", "12", "2222", "Vienna"));

            _readOnlyCustomersRepository.Setup(r => r.GetByGuid<Customer>(new Guid("a41b05f3-e11f-40a2-a0b6-91b56a11c5e4")))
                .Returns(customerToBeUpdated);

            _customerRepository.Setup(r => r.Update(customerToBeUpdated));

            // Act
            _unitToTestCustomerServiceCreate.Update(customerToBeUpdated);

            // Assert
            _customerRepository.Verify(r => r.Update(customerToBeUpdated), Times.Once);
        }

        // * CustomerServiceUpdateException successfully thrown
        [Fact]
        public void UpdateCustomer_CustomerServiceUpdateException_Test()
        {
            // Arrange
            Customer? customerToBeUpdated = new Customer(
                new Guid("a41b05f3-e11f-40a2-a0b6-91b56a11c5e4"),
                Genders.Female,
                "Cust First Name",
                "Cust Last Name",
                "cust@example.mail",
                123456,
                new DateTime(2023, 04, 02),
                new DateTime(1986, 08, 19),
                new Address("Street Example2", "12", "2222", "Vienna"));

            _readOnlyCustomersRepository.Setup(r => r.GetByGuid<Customer>(new Guid("a41b05f3-e11f-40a2-a0b6-91b56a11c5e4")))
                .Returns(null as Customer);

            // Act + Assert
            var ex = Assert.Throws<CustomerServiceUpdateException>(() => _unitToTestCustomerServiceCreate.Update(customerToBeUpdated));
            Assert.Equal("Update nicht möglich, der Kunde existiert nicht!", ex.Message);
        }
    }
}
