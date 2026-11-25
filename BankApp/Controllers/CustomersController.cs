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

        private static readonly Dictionary<string, string> NordicCountries = new()
        {
            { "Sweden", "SE" },
            { "Denmark", "DK" },
            { "Finland", "FI" },
            { "Norway", "NO" }
        };
        private List<SelectListItem> GetCountryOptions()
        {
            return NordicCountries
                .Select(c => new SelectListItem 
                { 
                    Value = c.Key, 
                    Text = c.Key 
                })
                .ToList();
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

            var countryCode = NordicCountries[model.Country];

            var dto = new NewCustomerDto
            (
                model.Gender, 
                model.Givenname, 
                model.Surname, 
                model.Streetaddress, 
                model.City, 
                model.Zipcode, 
                model.Country, 
                countryCode,
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

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var customerDto = await _customerService.GetCustomerDetailsDtoAsync(id); // Ska man kanske göra ny DTO och Service-metod?

            if (customerDto == null)
            {
                TempData["Error"] = $"Det finns ingen kund med kundnummer {id}.";
            }

            var model = new EditCustomerViewModel
            {
                CustomerId = customerDto.CustomerId,
                Givenname = customerDto.Givenname,
                Surname = customerDto.Surname,
                Gender = customerDto.Gender,
                Streetaddress = customerDto.Streetaddress,
                City = customerDto.City,
                Zipcode = customerDto.Zipcode,
                Country = customerDto.Country,
                //CountryCode = customerDto.CountryCode,
                Birthday = customerDto.Birthday,
                NationalId = customerDto.NationalId,
                Telephonenumber = customerDto.Telephonenumber,
                Emailaddress = customerDto.Emailaddress
            };

            FillDropDowns(model);

            return View(model);
        }        

        [HttpPost]
        public async Task<IActionResult> Edit(EditCustomerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                FillDropDowns(model);
                return View(model);
            }

            var countryCode = NordicCountries[model.Country];

            var dto = new EditCustomerDto
            (
                model.Gender,
                model.Givenname,
                model.Surname,
                model.Streetaddress,
                model.City,
                model.Zipcode,
                model.Country,
                countryCode,
                model.Birthday,
                model.NationalId,
                model.Telephonenumber,
                model.Emailaddress
            );

            try
            {
                await _customerService.UpdateCustomerAsync(model.CustomerId, dto);
            }
            catch (InvalidOperationException)
            {
                TempData["Error"] = $"Kunden {model.CustomerId} finns inte längre.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = $"Kunden {model.Givenname} {model.Surname} uppdaterades";

            return RedirectToAction(nameof(Details), new { id = model.CustomerId });
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

            model.CountryOptions = GetCountryOptions();
        }

        private void FillDropDowns(EditCustomerViewModel model)
        {
            model.GenderOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "male", Text = "Man" },
                new SelectListItem { Value = "female", Text = "Kvinna" },
            };

            model.CountryOptions = GetCountryOptions();
        }
    }
}
