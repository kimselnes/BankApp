using BankApp.Data.DTOs;

namespace BankApp.Data.Interfaces
{
    public interface ISuspiciousTransactionRepository
    {
        Task<IList<SuspiciousTransactionDto>> GetLargeTransactionsAsync(DateOnly fromDate, int onlyNewerThanId);
        Task<IList<SuspiciousTransactionDto>> GetHighTurnoverAccountsAsync(DateOnly fromDate);
    }
}
