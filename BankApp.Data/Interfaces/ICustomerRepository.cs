using BankApp.Data.DTOs;
using BankApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Data.Interfaces
{
    public interface ICustomerRepository
    {
        Task<PagedResult<CustomerSummaryDto>> SearchAsync(string? name, string? city, int PageNo, int PageSize);
        Task<CustomerDetailsDto?> GetDetailsAsync(int customerId);
    }
}
