﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
}