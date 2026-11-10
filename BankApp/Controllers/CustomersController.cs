using BankApp.Services.Interfaces;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BankApp.Controllers
{
    [Authorize(Roles ="Admin, Cashier")]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        public async Task<IActionResult> Index(string? name, string? city, string sortColumn, string sortOrder, int pageNo = 1)
        {
            if (pageNo < 1)
            {
                pageNo = 1;
            }

            var result = await _customerService.SearchCustomerAsync(name, city, pageNo, 50,  sortOrder,  sortColumn);

            return View(new CustomerIndexViewModel
            {
                Name = name,
                City = city,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
                Results = result

            });
        }

        public async Task<IActionResult> Details(int id) 
        {
            var customer = await _customerService.GetCustomerDetailsDtoAsync(id);

            if (customer == null)
            {
                TempData["Error"] = $"Det finns ingen kund med kundnummer {id}.";
                return RedirectToAction(nameof(Index));
            }

            return View(new CustomerDetailsViewModel { Customer = customer});
        }
    }
}
