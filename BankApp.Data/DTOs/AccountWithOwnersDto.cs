namespace BankApp.Data.DTOs
{
    public record AccountWithOwnersDto
    (
        int AccountId,
        decimal Balance,
        string Frequency,
        DateOnly Created,

        IReadOnlyList<AccountOwnerDto> Owners
    );
}
