using BankApp.Models;
using BankApp.Services.Interfaces;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BankApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStatisticsService _statisticsService;
        public HomeController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }


        public async Task<IActionResult> Index()
        {
            var stats = await _statisticsService.GetStatisticsPerCountryAsync();
            return View(new HomeIndexViewModel { Statistics = stats });
        }

        [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "country" })]
        public async Task<IActionResult> TopCustomers(string country)
        {
            var customers = await _statisticsService.GetTopCustomersByCountryAsync(country, 10);

            return View(new CountryTopCustomersViewModel
            {
                Country = country,
                Customers = customers
            });
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
