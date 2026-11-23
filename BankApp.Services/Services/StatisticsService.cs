using BankApp.Data.DTOs;
using BankApp.Data.Interfaces;
using BankApp.Services.Interfaces;

namespace BankApp.Services.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository _statisticsRepository;
        public StatisticsService(IStatisticsRepository statisticsRepository)
        {
            _statisticsRepository = statisticsRepository;
        }


        public async Task<IList<CountryStatsDto>> GetStatisticsPerCountryAsync()
        {
            return await _statisticsRepository.GetCountryStatsAsync();
        }

        public async Task<IList<CountryTopCustomerDto>> GetTopCustomersByCountryAsync(string country, int take)
        {
            return await _statisticsRepository.GetTopCustomersByCountryAsync(country, take);
        }
    }    
}
