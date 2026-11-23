using BankApp.Data.DTOs;
using BankApp.Data.Interfaces;
using BankApp.Services.Interfaces;

namespace BankApp.Services.Services
{
    public class SuspiciousTransactionService : ISuspiciousTransactionService
    {
        private readonly ISuspiciousTransactionRepository _repository;
        public SuspiciousTransactionService(ISuspiciousTransactionRepository repository)
        {
            _repository = repository;
        }

        public async Task<IList<SuspiciousTransactionDto>> GetSuspiciousTransactionsAsync(DateOnly fromDate, int onlyNewerThanId)
        {
            var large = await _repository.GetLargeTransactionsAsync(fromDate, onlyNewerThanId);
            var turnover = await _repository.GetHighTurnoverAccountsAsync(fromDate);

            var merged = large
                .Concat(turnover)
                .GroupBy(t => t.TransactionId)
                .Select(g => g.First())
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.TransactionId)
                .ToList();

            return merged;
        }
    }
}
