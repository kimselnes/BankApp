using BankApp.Data.DTOs;
using BankApp.Data.Entities;
using BankApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Data.Interfaces
{
    public interface ICustomerRepository
    {
        Task<PagedResult<CustomerSummaryDto>> SearchAsync(string? name, string? city, int PageNo, int PageSize, string sortOrder, string sortColumn);
        Task<CustomerDetailsDto?> GetDetailsAsync(int customerId);

        Task<Customer?> GetCustomerForUpdateAsync(int customerId);
        void AddCustomer(Customer customer);
        Task SaveChangesAsync();
    }
}
