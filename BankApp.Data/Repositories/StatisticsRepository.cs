using BankApp.Data.Data;
using BankApp.Data.DTOs;
using BankApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Text;

namespace BankApp.Data.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly BankAppDataContext _dbContext;
        public StatisticsRepository(BankAppDataContext dbContext)
            => _dbContext = dbContext;


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

        //Task<IList<CountryStatsDto>> IStatisticsRepository.GetCountryStatsAsync()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
