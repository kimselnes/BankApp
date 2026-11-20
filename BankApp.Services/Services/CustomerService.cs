using BankApp.Data.DTOs;
using BankApp.Data.Entities;
using BankApp.Data.Interfaces;
using BankApp.Data.Models;
using BankApp.Services.Interfaces;

namespace BankApp.Services.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;

        public CustomerService(ICustomerRepository customerRepository, IAccountRepository accountRepository)
        {
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
        }

        public async Task CreateCustomerAsync(NewCustomerDto dto)
        {
            var customer = new Customer
            {
                Givenname = dto.Givenname,
                Surname = dto.Surname,
                Gender = dto.Gender,
                Streetaddress = dto.Streetaddress,
                City = dto.City,
                Zipcode = dto.Zipcode,
                Country = dto.Country,
                CountryCode = dto.CountryCode,
                Birthday = dto.Birthday,
                NationalId = dto.NationalId,
                Telephonenumber = dto.Telephonenumber,
                Emailaddress = dto.Emailaddress
            };

            var account = new Account
            {
                Balance = 0,
                Frequency = dto.Frequency.ToString(),
                Created = DateOnly.FromDateTime(DateTime.Now)
            };

            account.Dispositions.Add(new Disposition
            {
                Customer = customer,
                Type = "OWNER"
            });

            _customerRepository.AddCustomer(customer);
            _accountRepository.AddAccount(account);

            await _customerRepository.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(int customerId, EditCustomerDto dto)
        {
            var customer = await _customerRepository.GetCustomerForUpdateAsync(customerId);

            if (customer == null)
            {
                throw new InvalidOperationException($"Kunden med ID {customerId} hittades inte.");
            }

            customer.Gender = dto.Gender;
            customer.Givenname = dto.Givenname;
            customer.Surname = dto.Surname;
            customer.Streetaddress = dto.Streetaddress;
            customer.City = dto.City;
            customer.Zipcode = dto.Zipcode;
            customer.Country = dto.Country;
            customer.CountryCode = dto.CountryCode;
            customer.Birthday = dto.Birthday;
            customer.NationalId = dto.NationalId;
            customer.Telephonenumber = dto.Telephonenumber;
            customer.Emailaddress = dto.Emailaddress;

            await _customerRepository.SaveChangesAsync();
        }

        public async Task<CustomerDetailsDto?> GetCustomerDetailsDtoAsync(int customerId)
        {
            return await _customerRepository.GetDetailsAsync(customerId);
        }

        public async Task<PagedResult<CustomerSummaryDto>> SearchCustomerAsync(string name, string city, int pageNo, int pageSize, string sortOrder, string sortColumn)
        {
            return await _customerRepository.SearchAsync(name, city, pageNo, pageSize, sortOrder, sortColumn);
        }
    }
}
