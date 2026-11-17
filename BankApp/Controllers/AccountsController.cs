using BankApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id, int pageNo = 1)
        {
            var accountDto = await _accountService.GetAccountAsync(id);

            if (accountDto == null)
            {
                TempData["Error"] = $"Det finns inget konto med kontonummer {id}.";
                return RedirectToAction(nameof(Index));
            }

            var transactions = await _accountService.GetTransactionsAsync(id, pageNo);

            return View(new ViewModels.AccountDetailsViewModel
            {
                Account = accountDto,
                Transactions = transactions
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions(int id, int pageNo)
        {
            var transactions = await _accountService.GetTransactionsAsync(id, pageNo);

            Response.Headers["X-Has-Next"] = transactions.HasNext.ToString().ToLowerInvariant();

            return PartialView("_TransactionRows", transactions.Items);
        }
    }
}
