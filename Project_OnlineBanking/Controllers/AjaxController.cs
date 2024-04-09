using Microsoft.AspNetCore.Mvc;
using Project_OnlineBanking.Services;
using Project_OnlineBanking.Models;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;

namespace Project_OnlineBanking.Controllers
{
    [Route("ajax")]
    public class AjaxController : Controller
    {
        private UserService userService;
        private TransactionService transactionService;
        private DatabaseContext db;
        public AjaxController(UserService _userService, TransactionService _transactionService, DatabaseContext _db)
        {
            userService = _userService;
            transactionService = _transactionService;
            db = _db;
        }

        [Route("fullnameByAccnum")]
        public IActionResult FullnameByAccountNumber(string accnum)
        {
            return new JsonResult(userService.findFullnameByAccnum(accnum));
        }

        [Route("mailOTP")]
        public IActionResult MailOTP()
        {
            string sender = userService.findByBankId((int)HttpContext.Session.GetInt32("bankId")).AccountNumber;
            var BaSender = db.BankAccounts.Where(n => n.AccountNumber == sender).FirstOrDefault();
            var mailotp = transactionService.mailOTP(BaSender.Account.Email);
            return new JsonResult(mailotp);
        }

        [Route("checkRegister")]
        public IActionResult CheckUsername(string username, string email)
        {
            return new JsonResult(userService.checkRegister(username, email));
        }

        [Route("chart")]
        public IActionResult Chart()
        {
            int accountId = 2;
            var transactions = transactionService.findByAccountId(accountId);

            object[] report = new object[12];
            foreach (var transaction in transactions)
            {
                DateOnly transactionDateOnly = DateOnly.FromDateTime((DateTime)transaction.TransactionDate);
                int month = transactionDateOnly.Month;
                for (int i = 1; i <= 12; i++)
                {
                    string mon = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i);
                    if (month == i)
                    {
                        report[i - 1] = @"{ ""month"":" + mon + @" , ""received"": " + transactionService.AmountUp(accountId) + @" , ""spent"": " + transactionService.AmountDown(accountId) + " }";
                    }
                    else
                    {
                        report[i -1] = @"{ ""month"":" + mon + @" , ""received"": 0 , ""spent"": 0 }";
                    }
                }
            }
            return new JsonResult(report);
        }
    }
}
