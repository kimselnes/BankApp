using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Data.DTOs
{
    public record EditCustomerDto
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
        string? Emailaddress
    );
}
