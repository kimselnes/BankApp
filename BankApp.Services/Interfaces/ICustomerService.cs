using BankApp.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDetailsDto> GetCustomerDetailsDtoAsync(int customerId);
    }
}
