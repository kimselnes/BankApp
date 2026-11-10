using BankApp.Data.DTOs;
using BankApp.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class CustomerIndexViewModel
    {
        [Display(Name = "Namn")]
        [StringLength(100)]
        public string? Name { get; set; }


        [Display(Name = "Stad")]
        [StringLength(50)]
        public string? City { get; set; }

        public PagedResult<CustomerSummaryDto> Results { get; set; } = new();
    }
}
