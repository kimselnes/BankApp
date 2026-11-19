using BankApp.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class NewCustomerViewModel
    {
        [Required(ErrorMessage = "Ange förnamn.")]
        [MaxLength(100, ErrorMessage = "Förnamn får inte vara längre än 100 tecken.")]
        [Display(Name = "Förnamn")]
        public string Givenname { get; set; }

        [Required(ErrorMessage = "Ange efternamn.")]
        [MaxLength(100, ErrorMessage = "Efternamn får inte vara längre än 100 tecken.")]
        [Display(Name = "Efternamn")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Ange kön.")]
        [Display(Name = "Kön")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Ange gatuadress.")]
        [MaxLength(100, ErrorMessage = "Gatuadress får inte vara längre än 100 tecken.")]
        [Display(Name = "Gatuadress")]
        public string Streetaddress { get; set; }

        [Required(ErrorMessage = "Ange stad.")]
        [MaxLength(100, ErrorMessage = "Stad får inte vara längre än 100 tecken.")]
        [Display(Name = "Stad")]
        public string City { get; set; }

        [Required(ErrorMessage = "Ange postnummer.")]
        [MaxLength(15, ErrorMessage = "Postnummer får inte vara längre än 15 tecken.")]
        [Display(Name = "Postnummer")]
        public string Zipcode { get; set; }

        [Required(ErrorMessage = "Ange land.")]
        [MaxLength(100, ErrorMessage = "Land får inte vara längre än 100 tecken.")]
        [Display(Name = "Land")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Ange landskod.")]
        [MaxLength(2, ErrorMessage = "Landskod får inte vara längre än 2 tecken.")]
        [Display(Name = "Landskod")]
        public string CountryCode { get; set; }

        [Display(Name = "Födelsedatum")]
        [DataType(DataType.Date)]
        public DateOnly? Birthday { get; set; }

        [MaxLength(20, ErrorMessage = "Personnummer får inte vara längre än 20 tecken.")]
        [Display(Name = "Personnummer")]
        public string? NationalId { get; set; }

        [MaxLength(25, ErrorMessage = "Telefonnummer får inte vara längre än 25 tecken.")]
        [Display(Name = "Telefonnummer")]
        public string? Telephonenumber { get; set; }

        [MaxLength(100, ErrorMessage = "E-postadress får inte vara längre än 100 tecken.")]
        [EmailAddress(ErrorMessage = "Ange en giltig e-postadress.")]
        [Display(Name = "E-post")]
        public string? Emailaddress { get; set; }

        [Required(ErrorMessage = "Ange kontotyp.")]
        [Display(Name = "Kontotyp (Frequency)")]
        public AccountFrequency Frequency { get; set; }

        public List<SelectListItem> FrequencyOptions { get; set; } = new();
        public List<SelectListItem> GenderOptions { get; set; } = new();
    }
}
