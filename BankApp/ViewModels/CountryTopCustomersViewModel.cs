using BankApp.Data.DTOs;

namespace BankApp.ViewModels
{
    public class CountryTopCustomersViewModel
    {
        public string Country { get; set; } = null!;
        public IList<CountryTopCustomerDto> Customers { get; set; } = new List<CountryTopCustomerDto>();
    }
}
