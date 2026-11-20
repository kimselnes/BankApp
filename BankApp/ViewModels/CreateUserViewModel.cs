using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Ange e-postadress.")]
        [EmailAddress(ErrorMessage = "Ange en giltig e-postadress.")]
        [Display(Name = "E-post")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Ange ett lösenord.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Lösenordet måste vara minst 6 tecken.")]
        [Display(Name = "Lösenord")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Bekräfta lösenordet.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Lösenorden stämmer inte överens.")]
        [Display(Name = "Bekräfta lösenord")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Välj en roll.")]
        [Display(Name = "Roll")]
        public string Role { get; set; } = null!;

        public List<SelectListItem> RoleOptions { get; set; } = new();
    }
}
