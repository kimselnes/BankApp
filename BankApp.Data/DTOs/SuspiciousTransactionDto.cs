namespace BankApp.Data.DTOs
{
    public record SuspiciousTransactionDto(
        int TransactionId,
        int AccountId,
        int CustomerId,
        string CustomerName,
        string Country,
        DateOnly Date,
        decimal Amount,
        string Type,
        string Operation,
        string Rule);
}
