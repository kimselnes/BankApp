using BankApp.Data.DTOs;

namespace BankApp.Data.Interfaces
{
    public interface IStatisticsRepository
    {
        Task<IList<CountryStatsDto>> GetCountryStatsAsync();

        Task<IList<CountryTopCustomerDto>> GetTopCustomersByCountryAsync(string country, int take);
    }
}
