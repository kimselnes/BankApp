using BankApp.Data.DTOs;
using BankApp.Data.Models;

namespace BankApp.Data.Interfaces
{
    public interface IAccountRepository
    {
        Task<AccountWithOwnersDto?> GetAccountWithOwnersAsync(int accountId);
        Task<PagedResult<TransactionDto>> GetTransactionsAsync(int accountId, int pageNo, int pageSize);
    }
}
