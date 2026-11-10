using BankApp.Data.Data;
using BankApp.Data.DTOs;
using BankApp.Data.Entities;
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

        public async Task<PagedResult<CustomerSummaryDto>> SearchAsync(string? name, string? city, int pageNo, int pageSize, string sortOrder, string sortColumn)
        {
            if (pageNo < 1)
            {
                pageNo = 1;
            }
            if (pageSize < 1)
            {
                pageSize = 50;
            }

            IQueryable<Customer> query = _dbContext.Customers;

            if (!string.IsNullOrWhiteSpace(name)) // If name has text.
            {
                var n = name.Trim();
                query = query.Where(c => c.Givenname.Contains(n) || c.Surname.Contains(n));
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                var ci = city.Trim();
                query = query.Where(c => c.City != null && c.City.Contains(ci));
            }

            // Sortering
            if (sortColumn == "CustomerId")
            {
                if (sortOrder == "desc")
                {
                    query = query.OrderByDescending(c => c.CustomerId);
                }
                else
                {
                    query = query.OrderBy(c => c.CustomerId);

                }
            }
            else
            {
                query = query
                    .OrderBy(c => c.Surname)
                    .ThenBy(c => c.Givenname)
                    .ThenBy(c => c.CustomerId);
            }

            var totalCount = await query.CountAsync();

            var items = await query
                //.OrderBy(c => c.Surname)
                //.ThenBy(c => c.Givenname)
                //.ThenBy(c => c.CustomerId)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CustomerSummaryDto(
                    c.CustomerId,
                    c.Givenname,
                    c.Surname,
                    c.NationalId,
                    c.Streetaddress,
                    c.City,
                    c.Zipcode,
                    c.Country
                    ))
                .ToListAsync();

            return new PagedResult<CustomerSummaryDto>
            {
                Items = items,
                TotalCount = totalCount,
                PageNo = pageNo,
                PageSize = pageSize
            };
        }
    }
}
