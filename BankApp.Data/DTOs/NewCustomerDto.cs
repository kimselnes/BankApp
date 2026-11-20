using BankApp.Data.Models;

namespace BankApp.Data.DTOs
{
    public record NewCustomerDto
    (
        string Gender,
        string Givenname,
        string Surname,
        string Streetaddress,
        string City,
        string Zipcode,
        string Country,
        string CountryCode,
        DateOnly? Birthday,
        string? NationalId,
        string? Telephonenumber,
        string? Emailaddress,
        AccountFrequency Frequency
    );
}
