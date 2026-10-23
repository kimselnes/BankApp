using BankApp.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankApp.Services.Interfaces
{
    internal interface IStatisticsService
    {
        Task<List<CountryStatsDto>> GetStatisticsPerCountryAsync();
    }
}
