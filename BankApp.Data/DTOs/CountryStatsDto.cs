using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Data.DTOs
{
    public record CountryStatsDto(
        string Country,
        int CustomerCount,
        int AccountCount,
        decimal TotalBalance);
}
