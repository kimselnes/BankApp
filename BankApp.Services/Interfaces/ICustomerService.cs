using BankApp.Data.DTOs;
using BankApp.Data.Models;

namespace BankApp.Services.Interfaces
{
    public interface ICustomerService
    {
        // Ska man lägga till ? efter CustomerDetailsDto ???
        Task<CustomerDetailsDto> GetCustomerDetailsDtoAsync(int customerId);
        Task<PagedResult<CustomerSummaryDto>> SearchCustomerAsync(string name, string city, int pageNo, int pageSize, string sortOrder, string sortColumn);

        //Task<TransactionResult> AddCustomerAsync(CustomerDetailsDto customerDetailsDto);
        Task CreateCustomerAsync(NewCustomerDto dto);
        Task UpdateCustomerAsync(int customerId, EditCustomerDto dto);
    }
}
