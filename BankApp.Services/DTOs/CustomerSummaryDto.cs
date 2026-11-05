using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BankApp.Services.DTOs
{
    public record CustomerSummaryDto(
        int CustomerId,
        string Givenname,
        string Surname,
        string NationalId, // Göra denna och neråt Nullbara? 'string?'.
        string Streetaddress,
        string City,
        string Zipcode,
        string Country
        );
}
