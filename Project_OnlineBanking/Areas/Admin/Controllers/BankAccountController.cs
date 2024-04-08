using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_OnlineBanking.Models;
using Project_OnlineBanking.Services;

namespace AccountBanking.Areas.Admin.Controllers;
[Authorize(Roles = "Admin")]
[Area("admin")]
[Route("admin/bankaccount")]
public class BankAccountController : Controller
{
    private AccountService accountService ;

    private BankAccountService bankAccountService;

    private TransactionService transactionService;

    private RoleService roleService;

    public BankAccountController(AccountService _accountService, RoleService _roleService, BankAccountService _bankAccountService, TransactionService _transactionService)
    {
        accountService = _accountService;
        roleService = _roleService;
        bankAccountService = _bankAccountService;
        transactionService = _transactionService;
    }
    

    [Route("account")]
    public IActionResult Account()
    {

        ViewBag.accounts = accountService.findAll().Where(a => a.RoleId != 1);
        return View("Account");
    }

    [Route("listAccountBank/{AccountId}")]
    public IActionResult ListAccountBank(int AccountId)
    {

        ViewBag.bankAccounts = bankAccountService.findByAccountId(AccountId);
        return View("listAccountBank");
    }

    [Route("transaction/{bankAccountId}")]
    public IActionResult Transaction(int bankAccountId)
    {
        ViewBag.transactions = transactionService.findByBankAccountId(bankAccountId);
        return View("Transaction");
    }

    [Route("topUp")]
    public IActionResult TopUp(int bankAccountId)
    {
        ViewBag.account = new Account();
        return View("TopUp");
    }

    [Route("check")]
    public IActionResult Check(string accountNumber)
    {
        var bankAccount = bankAccountService.findByAccountNumber(accountNumber);
        if (bankAccount != null)
        {
            var account = accountService.findById(bankAccount.AccountId);
            ViewBag.account = account;
            ViewBag.accountNumber = accountNumber;
            return View("TopUp", new Transaction());
        }
        else
        {
            TempData["msg"] = "Not exist";
            ViewBag.account = new Account();
        }
        return View("TopUp");
    }

    [Route("topupAccount")]
    public IActionResult TopUpAccount(string accountNumber, string amount)
    {
        var transaction = new Transaction();
        var bankAccountAdmin = bankAccountService.findById(1);
        var bankAccount = bankAccountService.findByAccountNumber(accountNumber);
        transaction.Amount = Int32.Parse(amount);
        transaction.RecipientAccountId = bankAccount.BankAccountId;
        transaction.SenderAccountId = bankAccountAdmin.BankAccountId;
        transaction.Description = "Recharge";
        transaction.TransactionType = "Recharge";
        transaction.TransactionDate = DateTime.Now;
        if (transactionService.topUp(transaction))
        {
            bankAccount.Balance += Int32.Parse(amount);
            bankAccountService.addAmount(bankAccount);
            TempData["notification"] = "Success";
        }
        return RedirectToAction("topUp");
    }
}
