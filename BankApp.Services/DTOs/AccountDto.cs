using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Services.DTOs
{
    public record AccountDto(
        int AccountId,
        decimal Balance,
        string Frequency,
        DateOnly Created);
}
