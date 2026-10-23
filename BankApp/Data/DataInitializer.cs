using Microsoft.AspNetCore.Identity;

namespace BankApp.Data
{
    public static class DataInitializer
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            string[] roles = { "Admin", "Cashier" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var result = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException($"Kunde inte skapa rollen {role}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                    //await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
                // Roberts users
                await CreateUserAsync(userManager, "robert.johansson@admin.se", "Abc123#", "Admin");
                await CreateUserAsync(userManager, "robert.johansson@cashier.se", "Abc123#", "Cashier");

                // Mina test users
                await CreateUserAsync(userManager, "kim@admin.se", "Abc123#", "Admin");
                await CreateUserAsync(userManager, "kim@cashier.se", "Abc123#", "Cashier");
        }

        private static async Task CreateUserAsync(UserManager<IdentityUser> userManager, string email, string password, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Kunde inte skapa {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }

                if (!await userManager.IsInRoleAsync(user, role))
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }

        }
    }
}
