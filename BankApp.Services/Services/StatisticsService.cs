using BankApp.Data.Data;
using BankApp.Data.DTOs;
using BankApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Security.Cryptography;
using System.Text;

namespace BankApp.Services.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly BankAppDataContext _dbContext;

        public StatisticsService(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CountryStatsDto>> GetStatisticsPerCountryAsync()
        {
            var rows = await _dbContext.Dispositions
                .Select(d => new
                {
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

            //var rows = await _dbContext.Customers
            //    .GroupBy(c => c.Country)
            //    .Select(g => new
            //    {
            //        Country = g.Key,
            //        CustomerCount = g.Count(),
            //        AccountCount = g.SelectMany(c => c.Dispositions)
            //                        .Select(d => d.AccountId)
            //                        .Distinct()
            //                        .Count(),
            //        TotalBalance = g.SelectMany(c => c.Dispositions)
            //                        .Sum(d => d.Account.Balance)
            //    })
            //    .OrderBy(x => x.Country)
            //    .ToListAsync();

            //return rows.Select(x => new CountryStatsDto(
            //    x.Country, x.CustomerCount, x.AccountCount, x.TotalBalance));
        }
    }
}
