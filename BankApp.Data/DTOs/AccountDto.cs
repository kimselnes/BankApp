namespace BankApp.Data.DTOs
{
    public record AccountDto
    (
        int AccountId,
        decimal Balance,
        string Frequency,
        DateOnly Created
    );
}
