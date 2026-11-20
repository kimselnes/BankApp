using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; } = null!;


        [Required(ErrorMessage = "Välj en e-post.")]
        [Display(Name = "E-post")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Välj en roll.")]
        [Display(Name = "Roll")]
        public string Role { get; set; } = null!;

        public List<SelectListItem> RoleOptions { get; set; } = new();
    }
}
