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
            model.RoleOptions = GetRoleOptions();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.RoleOptions = GetRoleOptions();
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
                AddError(createResult);

                model.RoleOptions = GetRoleOptions();
                return View(model);
            }

            var roleResult = await _userManager.AddToRoleAsync(user, model.Role);
            if (!roleResult.Succeeded)
            {
                AddError(roleResult);
                model.RoleOptions = GetRoleOptions();
                return View(model);
            }

            TempData["Success"] = $"Användaren {model.Email} skapades med rollen {model.Role}.";

            //await _userManager.AddToRoleAsync(user, model.Role);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                TempData["Error"] = "Användaren kunde inte hittas.";
                return RedirectToAction(nameof(Index));
            }

            var roles = await _userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email ?? user.UserName ?? "(okänd)",
                Role = roles.FirstOrDefault() ?? string.Empty
            };

            model.RoleOptions = GetRoleOptions();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.RoleOptions = GetRoleOptions();

                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                TempData["Error"] = "Användaren finns inte längre.";
                return RedirectToAction(nameof(Index));
            }

            var currentRoles = await _userManager.GetRolesAsync(user);

            var toRemove = currentRoles.Where(r => r != model.Role).ToList();
            if (toRemove.Count > 0)
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, toRemove);
                if (!removeResult.Succeeded)
                {
                    AddError(removeResult);
                    model.RoleOptions = GetRoleOptions();
                    return View(model);
                }
            }

            if (!currentRoles.Contains(model.Role))
            {
                var addResult = await _userManager.AddToRoleAsync(user, model.Role);
                if (!addResult.Succeeded)
                {
                    AddError(addResult);
                    model.RoleOptions = GetRoleOptions();
                    return View(model);
                }
            }

            TempData["Success"] = $"Användaren {model.Email} uppdaterades.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Error"] = "Användaren kunde inte hittas.";
                return RedirectToAction(nameof(Index));
            }

            var model = new DeleteUserViewModel
            {
                Id = user.Id,
                Email = user.Email ?? user.UserName ?? "(okänd)",
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            };

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                TempData["Error"] = "Användaren finns inte längre.";
                return RedirectToAction(nameof(Index));
            }

            if (user.Id == _userManager.GetUserId(User))
            {
                TempData["Error"] = "Du kan inte ta bort ditt eget konto.";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                AddError(result);
                return RedirectToAction(nameof(Index));
                //return View("Delete", new DeleteUserViewModel
                //{
                //    Id = user.Id,
                //    Email = user.Email ?? user.UserName ?? "(okänd)",
                //    Roles = (await _userManager.GetRolesAsync(user)).ToList()
                //});
            }

            TempData["Success"] = $"Användaren {user.Email} togs bort.";
            return RedirectToAction(nameof(Index));
        }

        private void AddError(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private List<SelectListItem> GetRoleOptions()
        {
            return _roleManager.Roles
                .Select(r => new SelectListItem
                {
                    Value = r.Name,
                    Text = r.Name
                }).ToList();
        }

        //private void FillRoleOptions(CreateUserViewModel model)
        //{
        //    model.RoleOptions = _roleManager.Roles
        //        .Select(r => new SelectListItem
        //        {
        //            Value = r.Name,
        //            Text = r.Name
        //        }).ToList();
        //}
    }
}
