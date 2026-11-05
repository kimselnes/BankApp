using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Data.DTOs
{
    public record CustomerDetailsDto(
        int CustomerId,
        string Givenname,
        string Surname,
        string Streetaddress,
        string City,
        string Zipcode,
        string Country,
        string CountryCode,
        string Gender,
        DateOnly? Birthday,
        string? NationalId,
        string? Telephonenumber,
        string? Emailaddress,
        IReadOnlyList<AccountDto> Accounts)
    {
        public decimal TotalBalance => Accounts.Sum(a => a.Balance);
    }

}
