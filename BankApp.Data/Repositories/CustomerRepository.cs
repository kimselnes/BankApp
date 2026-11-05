using BankApp.Data.DTOs;
using BankApp.Data.Interfaces;
using BankApp.Data.Models;
//using System;
//using System.Collections.Generic;
//using System.Text;

namespace BankApp.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        public Task<CustomerDetailsDto?> GetDetailsAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResult<CustomerSummaryDto>> SearchAsync(string? name, string? city, int PageNo, int PageSize)
        {
            throw new NotImplementedException();
        }
    }
}
