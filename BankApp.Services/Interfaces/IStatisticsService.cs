using BankApp.Data.DTOs;

namespace BankApp.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<IList<CountryStatsDto>> GetStatisticsPerCountryAsync();

        Task<IList<CountryTopCustomerDto>> GetTopCustomersByCountryAsync(string country, int take);
    }
}
