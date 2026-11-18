using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class TransferViewModel
    {
        public int FromAccountId { get; set; }

        [Display(Name = "Från kontonummer")]
        public int DisplayFromAccountId => FromAccountId; // Istället för att behöva skriva ut @Model.FromAccountId i vyn, kan vi använda DisplayFromAccountId för att visa kontonumret.

        public decimal Balance { get; set; } // Source/from account balance.


        [Required(ErrorMessage = "Ange ett mottagarkonto.")]
        [Display(Name = "Till kontonummer")]
        public int ToAccountId { get; set; }

        [Required(ErrorMessage = "Ange ett belopp.")]
        [Range(1, 100000, ErrorMessage = "Beloppet måste vara mellan 1 och 100000 kr.")]
        [Display(Name = "Belopp")]
        public decimal Amount { get; set; }
    }
}
