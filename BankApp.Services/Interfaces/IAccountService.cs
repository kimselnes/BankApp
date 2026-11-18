using BankApp.Data.DTOs;
using BankApp.Data.Models;

namespace BankApp.Services.Interfaces
{
    public interface IAccountService
    {
        Task<AccountWithOwnersDto?> GetAccountAsync(int accountId);
        Task<PagedResult<TransactionDto>> GetTransactionsAsync(int accountId, int pageNo);

        Task<TransactionResult> WithdrawAsync(int accountId, decimal amount);
        Task<TransactionResult> DepositAsync(int accountId, decimal amount);
        Task<TransactionResult> TransferAsync(int fromAccountId, int toAccountId, decimal amount);
    }
}
