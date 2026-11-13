

namespace BankApp.Data.DTOs
{
    public record TransactionDto
    (
        int TransactionId,
        DateOnly Date,
        string Type,
        string Operation,
        decimal Amount,
        decimal BalanceAfterTransaction,
        string? Symbol,
        string? Bank,
        string? RecipientAccountNumber
    );

    // Date only shows the day of transaction, not the time.
    // Therefore I should also OrderBy the TransactionId.
    // Otherwise transactions made the same day could disappear(?).

}
