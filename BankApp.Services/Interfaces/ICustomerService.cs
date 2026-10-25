using BankApp.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Services.Interfaces
{
    public interface ICustomerService
    {
        // Ska man lägga till ? efter CustomerDetailsDto ???
        Task<CustomerDetailsDto> GetCustomerDetailsDtoAsync(int customerId);
    }
}
