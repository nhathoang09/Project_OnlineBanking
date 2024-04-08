﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_OnlineBanking.Models;
using Project_OnlineBanking.Services;
using System.Diagnostics;

namespace Project_OnlineBanking.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private UserService userService;
        private TransactionService transactionService;
        private DatabaseContext db;

        public AccountController(UserService _userService, TransactionService _transactionService, DatabaseContext _db)
        {
            userService = _userService;
            transactionService = _transactionService;
            db = _db;
        }

        [Route("~/")]
        [Route("")]
        [Route("login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("login")]
        public IActionResult Login(string username, string password)
        {
            var account = userService.findByUsername(username);
            int failedSuccessCount = account.FailedLoginCount;
            HttpContext.Session.SetInt32("fsc", failedSuccessCount);
            if(account.RoleId == 1)
            {
                if (userService.Login(username, password))
                {
                    account.LastLoginSuccess = DateTime.Now;
                    HttpContext.Session.SetString("username", username);
                    return RedirectToAction("Index", "owner", new { area = "admin"});
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            else if (account.RoleId == 2)
            {
                if (userService.Login(username, password))
                {
                    account.LastLoginSuccess = DateTime.Now;
                    account.FailedLoginCount = 0;
                    db.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    HttpContext.Session.SetString("username", username);
                    return RedirectToAction("Middle");
                }
                else
                {
                    account.FailedLoginCount += 1;
                    db.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    TempData["username"] = username;
                    TempData["msg"] = false;
                    return RedirectToAction("Login");
                }
            }
            else
            {
                return RedirectToAction("Login");
            };
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            return RedirectToAction("Login");
        }

        [Route("register")]
        public IActionResult Register()
        {
            var account = new Account();
            return View("Register", account);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(Account account)
        {
            account.LastLoginSuccess = DateTime.Now;
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            account.RoleId = 2;
            account.IsTransferEnabled = false;
            account.FailedLoginCount = 0;
            if (userService.Create(account))
            {
                TempData["username"] = account.Username;
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Register");
            }
        }

        [Route("middle")]
        public IActionResult Middle()
        {
            ViewBag.accnums = userService.findByAccId(userService.findByUsername(HttpContext.Session.GetString("username")).AccountId);
            return View("Middle");
        }

        [HttpPost]
        [Route("middle")]
        public IActionResult Middle(int bankId)
        {
            HttpContext.Session.SetInt32("bankId", bankId);
            return RedirectToAction("Index", "Home");

        }

        [Route("banknumber")]
        public IActionResult BankNumber()
        {
            var bankacc = new BankAccount();
            return View("banknumber", bankacc);
        }

        [HttpPost]
        [Route("banknumber")]
        public IActionResult BankNumber(BankAccount bankacc)
        {
            bankacc.AccountId = userService.findByUsername(HttpContext.Session.GetString("username")).AccountId;
            bankacc.Balance = 0;
            if (userService.BankNumber(bankacc))
            {
                HttpContext.Session.SetInt32("bankId", bankacc.BankAccountId);
                return RedirectToAction("index", "home");
            }
            else
            {
                TempData["msg"] = "Failed!";
                return RedirectToAction("banknumber");
            }

        }

        [HttpPost]
        [Route("transfer")]
        public IActionResult Transfer(string receiver, decimal amount, string message)
        {
            string sender = userService.findByBankId((int)HttpContext.Session.GetInt32("bankId")).AccountNumber;
            transactionService.TransferMoney(sender, receiver, amount, message);
            return RedirectToAction("index", "home");

        }
    }
}
