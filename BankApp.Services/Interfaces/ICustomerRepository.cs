using BankApp.Services.DTOs;
using BankApp.Services.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Services.Interfaces
{
    public interface ICustomerRepository
    {
        Task<PagedResult<CustomerSummaryDto>> SearchAsync(string? name, string? city, int PageNo, int PageSize);
        Task<CustomerDetailsDto?> GetDetailsAsync(int customerId);
    }
}
