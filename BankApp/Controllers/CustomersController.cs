using BankApp.Data.DTOs;
using BankApp.Data.Models;
using BankApp.Services.Interfaces;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankApp.Controllers
{
    [Authorize(Roles = "Admin,Cashier")]
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

            var result = await _customerService.SearchCustomerAsync(name, city, pageNo, 50, sortOrder, sortColumn);

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
            var customerDto = await _customerService.GetCustomerDetailsDtoAsync(id);

            if (customerDto == null)
            {
                TempData["Error"] = $"Det finns ingen kund med kundnummer {id}.";
                return RedirectToAction(nameof(Index));
            }

            return View(new CustomerDetailsViewModel { Customer = customerDto });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new NewCustomerViewModel();
            FillDropDowns(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewCustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                FillDropDowns(model);
                return View(model);
            }

            var dto = new NewCustomerDto
            (
                model.Gender, 
                model.Givenname, 
                model.Surname, 
                model.Streetaddress, 
                model.City, 
                model.Zipcode, 
                model.Country, 
                model.CountryCode,
                model.Birthday, 
                model.NationalId, 
                model.Telephonenumber,
                model.Emailaddress, 
                model.Frequency
            );

            await _customerService.CreateCustomerAsync(dto);

            TempData["Success"] = $"Kunden {model.Givenname} {model.Surname} skapades";
            return RedirectToAction(nameof(Index));
        }

        private void FillDropDowns(NewCustomerViewModel model)
        {
            model.GenderOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "male", Text = "Man" },
                new SelectListItem { Value = "female", Text = "Kvinna" },
            };

            model.FrequencyOptions = Enum.GetValues<AccountFrequency>()
                .Select(f => new SelectListItem
                {
                    Value = f.ToString(),
                    Text = f.ToString()
                })
                .ToList();
        }
    }
}
