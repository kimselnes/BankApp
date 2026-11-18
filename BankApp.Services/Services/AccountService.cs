using BankApp.Data.DTOs;
using BankApp.Data.Interfaces;
using BankApp.Data.Models;
using BankApp.Services.Interfaces;
using BankApp.Data.Entities;

namespace BankApp.Services.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }


        private readonly int TransactionsPageSize = 20;

        public async Task<AccountWithOwnersDto?> GetAccountAsync(int accountId)
        {
            return await _accountRepository.GetAccountWithOwnersAsync(accountId);
        }

        public async Task<PagedResult<TransactionDto>> GetTransactionsAsync(int accountId, int pageNo)
        {
            return await _accountRepository.GetTransactionsAsync(accountId, pageNo, TransactionsPageSize);
        }

        public async Task<TransactionResult> WithdrawAsync(int accountId, decimal amount)
        {
            var account = await _accountRepository.GetAccountForUpdateAsync(accountId);

            if (account == null)
            {
                return TransactionResult.Fail(TransactionError.AccountNotFound, "Kontot hittades inte.");
            }

            if (amount <= 0)
            {
                return TransactionResult.Fail(TransactionError.IncorrectAmount, "Ange ett belopp större än 0 kr.");
            }

            if (amount > account.Balance)
            {
                return TransactionResult.Fail(TransactionError.BalanceTooLow, $"Kontot har bara {account.Balance:N2} kr. Uttaget går inte igenom.");
            }
            
            account.Balance -= amount;

            _accountRepository.AddTransaction(new Transaction
            {
                AccountId = account.AccountId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Type = "Debit",
                Operation = "Withdrawal in Cash",
                Amount = -amount,
                Balance = account.Balance,
            });

            await _accountRepository.SaveChangesAsync();

            return TransactionResult.Ok(); // Success() istället?!
        }

        public async Task<TransactionResult> DepositAsync(int accountId, decimal amount)
        {
            var account = await _accountRepository.GetAccountForUpdateAsync(accountId);

            if (account == null)
            {
                return TransactionResult.Fail(TransactionError.AccountNotFound, "Kontot hittades inte.");
            }

            if (amount <= 0)
            {
                return TransactionResult.Fail(TransactionError.IncorrectAmount, "Ange ett belopp större än 0 kr.");
            }

            account.Balance += amount;

            _accountRepository.AddTransaction(new Transaction
            {
                AccountId = account.AccountId,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Type = "Credit",
                Operation = "Credit in Cash",
                Amount = amount,
                Balance = account.Balance,
            });

            await _accountRepository.SaveChangesAsync();

            return TransactionResult.Ok();
        }
    }
}
