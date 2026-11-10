using BankApp.Data.DTOs;
//using System;
//using System.Collections.Generic;
//using System.Text;

namespace BankApp.Services.Interfaces
{
    public interface IStatisticsService
    {
        Task<IList<CountryStatsDto>> GetStatisticsPerCountryAsync();
    }
}
