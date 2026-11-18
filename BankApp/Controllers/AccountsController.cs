using BankApp.Data.Models;
using BankApp.Services.Interfaces;
using BankApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankApp.Controllers
{
    [Authorize(Roles = "Admin, Cashier")]
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}

        public async Task<IActionResult> Details(int id, int pageNo = 1)
        {
            var accountDto = await _accountService.GetAccountAsync(id);

            if (accountDto == null)
            {
                TempData["Error"] = $"Det finns inget konto med kontonummer {id}.";
                return RedirectToAction("Index", "Customers");
            }

            var transactions = await _accountService.GetTransactionsAsync(id, pageNo);

            return View(new AccountDetailsViewModel
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

        [HttpGet]
        public async Task<IActionResult> Withdraw(int id)
        {
            var account = await _accountService.GetAccountAsync(id);

            if (account == null)
            {
                TempData["Error"] = $"Det finns inget konto med kontonummer {id}";
                return RedirectToAction("Index", "Customers"); // Ska den verkligen till index? Kanske details?
                //return RedirectToAction(nameof(Index));
            }

            return View(new WithdrawViewModel
            {
                AccountId = account.AccountId,
                Balance = account.Balance
            });
        }

        [HttpPost]
        public async Task<IActionResult> Withdraw(WithdrawViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var accountForView = await _accountService.GetAccountAsync(model.AccountId);
                if (accountForView != null)
                {
                    model.Balance = accountForView.Balance;
                }

                return View(model);
            }

            var result = await _accountService.WithdrawAsync(model.AccountId, model.Amount);

            if (result.Success)
            {
                TempData["Success"] = $"Uttaget på {model.Amount:N2} kr genomfördes";
                return RedirectToAction(nameof(Details), new { id = model.AccountId });
            }

            ModelState.AddModelError(nameof(model.Amount), result.Message);

            var account = await _accountService.GetAccountAsync(model.AccountId);
            if (account != null)
            {
                model.Balance = account.Balance;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Deposit(int id)
        {
            var account = await _accountService.GetAccountAsync(id);

            if (account == null)
            {
                TempData["Error"] = $"Det finns inget konto med kontonummer {id}";
                return RedirectToAction("Index", "Customers");
            }

            return View(new DepositViewModel
            {
                AccountId = account.AccountId,
                Balance = account.Balance
            });
        }

        [HttpPost]
        public async Task<IActionResult> Deposit(DepositViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var accountForView = await _accountService.GetAccountAsync(model.AccountId);
                if (accountForView != null)
                {
                    model.Balance = accountForView.Balance;
                }

                return View(model);
            }

            var result = await _accountService.DepositAsync(model.AccountId, model.Amount);

            if (result.Success)
            {
                TempData["Success"] = $"Insättningen på {model.Amount:N2} kr genomfördes";
                return RedirectToAction(nameof(Details), new { id = model.AccountId });
            }

            ModelState.AddModelError(nameof(model.Amount), result.Message);

            var account = await _accountService.GetAccountAsync(model.AccountId);
            if (account != null)
            {
                model.Balance = account.Balance;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Transfer(int id)
        {
            var account = await _accountService.GetAccountAsync(id);

            if (account == null)
            {
                TempData["Error"] = $"Det finns inget konto med kontonummer {id}";
                return RedirectToAction("Index", "Customers");
            }

            return View(new TransferViewModel
            {
                FromAccountId = account.AccountId,
                Balance = account.Balance
            });
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(TransferViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var accountForView = await _accountService.GetAccountAsync(model.FromAccountId);
                if (accountForView != null)
                {
                    model.Balance = accountForView.Balance;
                }

                return View(model);
            }

            var result = await _accountService.TransferAsync(model.FromAccountId, model.ToAccountId, model.Amount);

            if (result.Success)
            {
                TempData["Success"] = $"Överföringen på {model.Amount:N2} kr genomfördes";
                return RedirectToAction(nameof(Details), new { id = model.FromAccountId });
            }

            var field = result.Error == TransactionError.BalanceTooLow
                ? nameof(model.Amount)
                : nameof(model.ToAccountId);

            ModelState.AddModelError(field, result.Message);
            //ModelState.AddModelError(nameof(model.Amount), result.Message);

            var account = await _accountService.GetAccountAsync(model.FromAccountId);
            if (account != null)
            {
                model.Balance = account.Balance;
            }

            return View(model);
        }
    }
}
