using BankApp.Data.Data;
using BankApp.Data.DTOs;
using BankApp.Data.Interfaces;
using BankApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly BankAppDataContext _dbContext;

        public CustomerRepository(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CustomerDetailsDto?> GetDetailsAsync(int customerId)
        {
            return await _dbContext.Customers
                .AsNoTracking()
                .Where(c => c.CustomerId == customerId)
                .Select(c => new CustomerDetailsDto(
                    c.CustomerId,
                    c.Givenname,
                    c.Surname,
                    c.Streetaddress,
                    c.City,
                    c.Zipcode,
                    c.Country,
                    c.CountryCode,
                    c.Gender,
                    c.Birthday,
                    c.NationalId,
                    c.Telephonenumber,
                    c.Emailaddress,
                    c.Dispositions.Select(d => new AccountDto(
                        d.Account.AccountId,
                        d.Account.Balance,
                        d.Account.Frequency,
                        d.Account.Created)).ToList()))
                .FirstOrDefaultAsync();
        }

        public Task<PagedResult<CustomerSummaryDto>> SearchAsync(string? name, string? city, int PageNo, int PageSize)
        {
            throw new NotImplementedException();
        }
    }
}
