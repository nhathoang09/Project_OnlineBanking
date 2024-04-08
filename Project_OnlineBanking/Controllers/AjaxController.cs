using Microsoft.AspNetCore.Mvc;
using Project_OnlineBanking.Services;
using Project_OnlineBanking.Models;
using System;
using static System.Net.Mime.MediaTypeNames;

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
        public IActionResult Chart(int accountId)
        {
            accountId = 2;
            var transactions = transactionService.findByAccountId(accountId);

            string[] report = new string[12];
            foreach (var transaction in transactions)
            {
                DateOnly transactionDateOnly = DateOnly.FromDateTime((DateTime)transaction.TransactionDate);
                int month = transactionDateOnly.Month;
                for (int i = 1; i <= 12; i++)
                {
                    if (month == i)
                    {
                        report[i-1] = "{ Month: " + i + ", Received: " + transactionService.AmountUp(accountId) + " , Spent: " + transactionService.AmountDown(accountId) + " }";
                    }
                    else
                    {
                        report[i-1] = "{ Month: " + i + ", Received: " + 0 + ", Spent: " + 0 + "}";
                    }
                }
            }
            return new JsonResult(report);
        }
    }
}
