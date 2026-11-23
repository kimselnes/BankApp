namespace BankApp.Data.DTOs
{
    public record CountryTopCustomerDto(
        int CustomerId,
        string Givenname,
        string Surname,
        decimal TotalBalance);
}
