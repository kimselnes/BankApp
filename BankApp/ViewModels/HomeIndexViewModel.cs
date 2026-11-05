using BankApp.Data.DTOs;

namespace BankApp.ViewModels
{
    public class HomeIndexViewModel
    {
        public IList<CountryStatsDto> Statistics { get; set; } = new List<CountryStatsDto>(); // Vill jag ha den som en IReadOnlyList ???
    }
}
