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

        public async Task<CustomerDetailsDto?> GetCustomerDetailsDtoAsync(int customerId)
        {
            return await _customerRepository.GetDetailsAsync(customerId);
        }

        public async Task<PagedResult<CustomerSummaryDto>> SearchCustomerAsync(string name, string city, int pageNo, int pageSize, string sortOrder, string sortColumn)
        {
            return await _customerRepository.SearchAsync(name, city, pageNo, pageSize, sortOrder, sortColumn);
        }



        //public async Task<CustomerDetailsDto> GetCustomerDetailsDtoAsync(int customerId)
        //{
        //    var customer = await _dbContext.Customers
        //        .Where(c => c.CustomerId == customerId)
        //        .Select(c => new
        //        {
        //            c.CustomerId,
        //            c.Givenname,
        //            c.Surname,
        //            c.Streetaddress,
        //            c.City,
        //            c.Zipcode,
        //            c.Country,
        //            c.CountryCode,
        //            c.Gender,
        //            c.Birthday,
        //            c.NationalId,
        //            c.Telephonenumber,
        //            c.Emailaddress,
        //            Accounts = c.Dispositions.Select(d => new AccountDto(
        //                d.Account.AccountId,
        //                d.Account.Balance,
        //                d.Account.Frequency,
        //                d.Account.Created)).ToList()

        //        }).FirstOrDefaultAsync();

        //    if (customer == null)
        //    {
        //        return null;
        //    }

        //    return new CustomerDetailsDto(
        //        customer.CustomerId,
        //        customer.Givenname,
        //        customer.Surname,
        //        customer.Streetaddress,
        //        customer.City,
        //        customer.Zipcode,
        //        customer.Country,
        //        customer.CountryCode,
        //        customer.Gender,
        //        customer.Birthday,
        //        customer.NationalId,
        //        customer.Telephonenumber,
        //        customer.Emailaddress,
        //        customer.Accounts.Sum(a => a.Balance),
        //        customer.Accounts);
        //}
    }
}
