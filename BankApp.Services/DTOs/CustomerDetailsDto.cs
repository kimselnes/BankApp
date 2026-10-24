using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Services.DTOs
{
    public record CustomerDetailsDto(
        int CustomerId,
        string Givenname,
        string Surname,
        string Streetadress,
        string City,
        string Zipcode,
        string Country,
        string CountryCode,
        string Gender,
        DateOnly? Birthday,
        string? NationalId,
        string? Telephonenumber,
        string? Emailadress,
        decimal TotalBalance,
        IReadOnlyList<AccountDto> Accounts);
}
