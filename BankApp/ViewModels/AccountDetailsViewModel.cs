using BankApp.Data.DTOs;
using BankApp.Data.Models;

namespace BankApp.ViewModels
{
    public class AccountDetailsViewModel
    {
        public AccountWithOwnersDto Account { get; set; } = null;
        public PagedResult<TransactionDto> Transactions { get; set; } = new();
    }
}
