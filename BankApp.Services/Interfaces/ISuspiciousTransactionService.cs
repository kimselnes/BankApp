using BankApp.Data.DTOs;

namespace BankApp.Services.Interfaces
{
    public interface ISuspiciousTransactionService
    {
        Task<IList<SuspiciousTransactionDto>> GetSuspiciousTransactionsAsync(DateOnly fromDate, int onlyNewerThanId);
    }
}
