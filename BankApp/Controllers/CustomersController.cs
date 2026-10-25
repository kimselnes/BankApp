using BankApp.Services.Interfaces;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Index()
        {
            return View();
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
