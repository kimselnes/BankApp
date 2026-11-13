using BankApp.Data.DTOs;
using BankApp.Data.Models;

namespace BankApp.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountWithOwnersDto?> GetAccountAsync(int accountId);
        Task<PagedResult<TransactionDto>> GetTransactionsAsync(int accountId, int pageNo);
    }
}
