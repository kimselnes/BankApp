using BankApp.Data.Data;
using BankApp.Data.DTOs;
using BankApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Data.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly BankAppDataContext _dbContext;
        public StatisticsRepository(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<IList<CountryStatsDto>> GetCountryStatsAsync()
        {
            var rows = await _dbContext.Dispositions
                .AsNoTracking()
                .Select(d => new {
                    d.Customer.Country,
                    d.CustomerId,
                    d.AccountId,
                    d.Account.Balance
                })
                .ToListAsync();

            return rows
                .GroupBy(r => r.Country)
                .Select(g => new CountryStatsDto(
                    g.Key,
                    g.Select(r => r.CustomerId).Distinct().Count(),
                    g.Select(r => r.AccountId).Distinct().Count(),
                    g.Sum(r => r.Balance)))
                .OrderBy(s => s.Country)
                .ToList();
        }

        public async Task<IList<CountryTopCustomerDto>> GetTopCustomersByCountryAsync(string country, int take)
        {
            var rows = await _dbContext.Customers
                .Where(c => c.Country == country)
                .Select(c => new
                {
                    c.CustomerId,
                    c.Givenname,
                    c.Surname,
                    TotalBalance = c.Dispositions.Sum(d => d.Account.Balance)
                })
                .ToListAsync();

            return rows
                .OrderByDescending(r => r.TotalBalance)
                .ThenBy(r => r.CustomerId)
                .Take(take)
                .Select(r => new CountryTopCustomerDto(
                    r.CustomerId,
                    r.Givenname,
                    r.Surname,
                    r.TotalBalance
                    )).ToList();
        }
    }
}
