using Spg.FlowerShop.Domain.Exceptions;
using Spg.FlowerShop.Domain.Interfaces;
using Spg.FlowerShop.Domain.Model;
using System.Text.RegularExpressions;

namespace Spg.FlowerShop.Application.Customers
{
    public class CustomerService : IReadOnlyCustomerService, IAddableCustomerService, IDeletableCustomerService, IUpdateableCustomerService
    {
        private readonly IReadOnlyRepositoryGeneric<Customer> _readOnlyCustomersRepository;
        private readonly IRepositoryGeneric<Customer> _customerRepository;
        private readonly IDateTimeService _dateTimeService;

        private const int maxWordlLength = 32;

        public CustomerService(
            IReadOnlyRepositoryGeneric<Customer> readOnlyCustomersRepository, 
            IRepositoryGeneric<Customer> customerRepository,
            IDateTimeService dateTimeService
            )
        {
            _readOnlyCustomersRepository = readOnlyCustomersRepository;
            _customerRepository = customerRepository;
            _dateTimeService = dateTimeService;
        }

        public IQueryable<Customer> GetAll()
        {
            IQueryable<Customer> customers = _readOnlyCustomersRepository.GetAll();
            return customers;
        }

        public void Create(Customer newCustomer)
        {
            if (newCustomer == null)
            {
                throw new CustomerServiceCreateException("Fehlender Kunde");
            }

            if (newCustomer.Id != 0)
            {
                throw new CustomerServiceCreateException("Customer ID darf nur 0 sein");
            }

            if (string.IsNullOrWhiteSpace(newCustomer.FirstName) || newCustomer.FirstName.Length > maxWordlLength)
            {
                throw new CustomerServiceCreateException("Vorname ist ungültig");
            }

            if (string.IsNullOrWhiteSpace(newCustomer.LastName) || newCustomer.LastName.Length > maxWordlLength)
            {
                throw new CustomerServiceCreateException("Nachname ist ungültig");
            }

            if (string.IsNullOrEmpty(newCustomer.Email) || !(newCustomer.Email).Contains("@") || newCustomer.Email.Length > maxWordlLength)
            {
                throw new CustomerServiceCreateException("Email ist ungültig");
            }

            if (!newCustomer.Gender.Equals(Genders.Male) && !newCustomer.Gender.Equals(Genders.Female) && !newCustomer.Gender.Equals(Genders.Other))
            {
                throw new CustomerServiceCreateException("Geschlecht ist ungültig");                
            }

            if (newCustomer.RegistrationDateTime == DateTime.MinValue)
            {
                throw new CustomerServiceCreateException("Registrierungsdatum hat Defaultwert und ist ungültig");
            }

            if (newCustomer.RegistrationDateTime > _dateTimeService.Now)
            {
                throw new CustomerServiceCreateException("Registration Date ist ungültig, das Registrierungsdatum muss in der Vergangenheit liegen");
            }

            //if (string.IsNullOrWhiteSpace(newCustomer.Address?.Street)
            //    || string.IsNullOrWhiteSpace(newCustomer.Address?.Number)
            //    || string.IsNullOrWhiteSpace(newCustomer.Address?.Zip)
            //    || string.IsNullOrWhiteSpace(newCustomer.Address?.City))
            //{
            //    throw new CustomerServiceCreateException("Die Adrese ist ungültig");
            //}

            _customerRepository.Create(newCustomer);
        }

        public void Delete(Guid guidCustomerToBeDeletedGuid)
        {
            Customer? customerToBeDeleted = _readOnlyCustomersRepository.GetByGuid<Customer>(guidCustomerToBeDeletedGuid);
            if (customerToBeDeleted == null)
            {
                throw new CustomerServiceDeleteException("Der Kunde existiert nicht!");
            }
            _customerRepository.Delete(customerToBeDeleted);
        }

        public void Update(Customer customer)
        {
            if (_readOnlyCustomersRepository.GetByGuid<Customer>(customer.Guid) is null)
            {
                throw new CustomerServiceUpdateException("Update nicht möglich, der Kunde existiert nicht!");
            }

            _customerRepository.Update(customer);
        }
    }
}
