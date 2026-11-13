namespace BankApp.Data.DTOs
{
    public record AccountOwnerDto
    (
        int CustomerId,
        string Givenname,
        string Surname
    );
}
