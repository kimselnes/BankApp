using BankApp.Data.Data;
using BankApp.Data.DTOs;
using BankApp.Data.Entities;
using BankApp.Data.Interfaces;
using BankApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BankAppDataContext _dbContext;

        public AccountRepository(BankAppDataContext dbContext)
        {
            _dbContext = dbContext;
        }



        public async Task<AccountWithOwnersDto?> GetAccountWithOwnersAsync(int accountId)
        {
            return await _dbContext.Accounts
                .Where(a => a.AccountId == accountId)
                .Select(a => new AccountWithOwnersDto(
                    a.AccountId,
                    a.Balance,
                    a.Frequency,
                    a.Created,
                    a.Dispositions.Select(d => new AccountOwnerDto(
                        d.Customer.CustomerId,
                        d.Customer.Givenname,
                        d.Customer.Surname)).ToList()))
                .FirstOrDefaultAsync();
        }

        public async Task<PagedResult<TransactionDto>> GetTransactionsAsync(int accountId, int pageNo, int pageSize)
        {
            var query = _dbContext.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.TransactionId); //Avoid placement issues when paging. (Also ensures better sorting in the list, not perfect though.)

            var totalCount = await query.CountAsync();

            var transactions = await query
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TransactionDto(
                    t.TransactionId,
                    t.Date,
                    t.Type,
                    t.Operation,
                    t.Amount,
                    t.Balance,
                    t.Symbol,
                    t.Bank,
                    t.Account))
                .ToListAsync();

            return new PagedResult<TransactionDto>
            {
                Items = transactions,
                TotalCount = totalCount,
                PageNo = pageNo,
                PageSize = pageSize
            };
        }

        public void AddTransaction(Transaction transaction)
        {
            _dbContext.Transactions.Add(transaction);
        }

        public async Task<Account?> GetAccountForUpdateAsync(int accountId)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId);
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
