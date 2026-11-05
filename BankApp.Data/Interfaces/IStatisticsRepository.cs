using BankApp.Data.DTOs;
//using System;
//using System.Collections.Generic;
//using System.Text;

namespace BankApp.Data.Interfaces
{
    public interface IStatisticsRepository
    {
        Task<IList<CountryStatsDto>> GetCountryStatsAsync();
    }
}
