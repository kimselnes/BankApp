using System.ComponentModel.DataAnnotations;

namespace BankApp.ViewModels
{
    public class WithdrawViewModel
    {
        public int AccountId { get; set; }

        [Display(Name = "Kontonummer")]
        public int DisplayAccountId => AccountId; // Istället för att behöva skriva ut @Model.AccountId i vyn, kan vi använda DisplayAccountId för att visa kontonumret.

        public decimal Balance { get; set; }

        [Required(ErrorMessage = "Ange ett belopp")]
        [Range(1, 100000, ErrorMessage = "Beloppet måste vara mellan 1 och 100000 kr")]
        [Display(Name = "Belopp")]
        public decimal Amount { get; set; }
    }
}
