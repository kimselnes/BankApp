using BankApp.Data.Data;
using BankApp.Data.DTOs;
using BankApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Data.Repositories
{
    public class SuspiciousTransactionRepository : ISuspiciousTransactionRepository
    {
        private readonly BankAppDataContext _dbContext;
        public SuspiciousTransactionRepository(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        private decimal LargeTransactionLimit = 15000m;
        private decimal HighTurnoverLimit = 23000m;


        public async Task<IList<SuspiciousTransactionDto>> GetLargeTransactionsAsync(DateOnly fromDate, int onlyNewerThanId)
        {
            var rows = await _dbContext.Transactions
                .Where(t => t.Date >= fromDate && t.TransactionId > onlyNewerThanId)
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.TransactionId)
                .Select(t => new
                {
                    t.TransactionId,
                    t.AccountId,
                    t.Date,
                    t.Amount,
                    t.Type,
                    t.Operation,
                    Country = t.AccountNavigation.Dispositions
                        .Select(d => d.Customer.Country).FirstOrDefault(),
                    CustomerId = t.AccountNavigation.Dispositions
                        .Where(d => d.Type == "OWNER")
                        .Select(d => d.CustomerId).FirstOrDefault(),
                    CustomerName = t.AccountNavigation.Dispositions
                        .Where(d => d.Type == "OWNER")
                        .Select(d => d.Customer.Givenname + " " + d.Customer.Surname)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return rows
                .Where(r => Math.Abs(r.Amount) > LargeTransactionLimit)
                .Select(r => new SuspiciousTransactionDto(
                    r.TransactionId,
                    r.AccountId,
                    r.CustomerId,
                    r.CustomerName ?? "(okänd)",
                    r.Country ?? "(okänd)",
                    r.Date,
                    r.Amount,
                    r.Type,
                    r.Operation,
                    "Enskild transaktion > 15 000 kr"))
                .ToList();
        }

        public async Task<IList<SuspiciousTransactionDto>> GetHighTurnoverAccountsAsync(DateOnly fromDate)
        {
            var rows = await _dbContext.Transactions
                .Where(t => t.Date >= fromDate)
                .Select(t => new
                {
                    t.TransactionId,
                    t.AccountId,
                    t.Date,
                    t.Amount,
                    t.Type,
                    t.Operation,
                    Country = t.AccountNavigation.Dispositions
                        .Select(d => d.Customer.Country).FirstOrDefault(),
                    CustomerId = t.AccountNavigation.Dispositions
                        .Where(d => d.Type == "OWNER")
                        .Select(d => d.CustomerId).FirstOrDefault(),
                    CustomerName = t.AccountNavigation.Dispositions
                        .Where(d => d.Type == "OWNER")
                        .Select(d => d.Customer.Givenname + " " + d.Customer.Surname)
                        .FirstOrDefault()
                })
                .ToListAsync();

            var flaggedAccounts = rows
                .GroupBy(r => r.AccountId)
                .Where(g => g.Sum(r => Math.Abs(r.Amount)) > HighTurnoverLimit)
                .Select(g => g.Key)
                .ToHashSet();

            return rows
                .Where(r => flaggedAccounts.Contains(r.AccountId))
                .Select(r => new SuspiciousTransactionDto(
                    r.TransactionId, 
                    r.AccountId, 
                    r.CustomerId,
                    r.CustomerName ?? "(okänd)", 
                    r.Country ?? "(okänt)",
                    r.Date, 
                    r.Amount, 
                    r.Type, 
                    r.Operation, 
                    "Kontots summa > 23 000 kr / 3 dygn"))
                .ToList();
        }
    }
}
