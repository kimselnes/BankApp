using BankApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BankApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }



        public async Task<IActionResult> Index()
        {
            var model = new UserListViewModel();

            foreach (var user in _userManager.Users.ToList())
            {
                var roles = await _userManager.GetRolesAsync(user);
                model.Users.Add(new UserListViewModel.UserRow
                {
                    Id = user.Id,
                    Email = user.Email ?? user.UserName ?? "(okänd)",
                    Roles = roles.ToList()
                });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateUserViewModel();
            FillRoleOptions(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                FillRoleOptions(model);
                return View(model);
            }

            var user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var createResult = await _userManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                FillRoleOptions(model);
                return View(model);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
            if (!roleResult.Succeeded)
            {
                foreach (var error in roleResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                FillRoleOptions(model);
                return View(model);
            }

            TempData["Success"] = $"Användaren {model.Email} skapades med rollen {model.Role}.";

            //await _userManager.AddToRoleAsync(user, model.Role);

            return RedirectToAction(nameof(Index));
        }

        private void FillRoleOptions(CreateUserViewModel model)
        {
            model.RoleOptions = _roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList();
        }
    }
}
