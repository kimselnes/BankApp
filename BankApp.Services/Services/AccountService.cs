using BankApp.Data.DTOs;
using BankApp.Data.Interfaces;
using BankApp.Data.Models;
using BankApp.Services.Interfaces;

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
        
    }
}
