using BankApp.Services.DTOs;

namespace BankApp.ViewModels
{
    public class HomeIndexViewModel
    {
        public IReadOnlyList<CountryStatsDto> Statistics { get; set; } = new List<CountryStatsDto>();
    }
}
