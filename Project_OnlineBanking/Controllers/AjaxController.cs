using Microsoft.AspNetCore.Mvc;
using Project_OnlineBanking.Services;
using Project_OnlineBanking.Models;

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

        [Route("checkOTP")]
        public IActionResult CheckOTP(string otp)
        {
            string sender = userService.findByBankId((int)HttpContext.Session.GetInt32("bankId")).AccountNumber;
            var BaSender = db.BankAccounts.Where(n => n.AccountNumber == sender).FirstOrDefault();
            var mail = transactionService.mailOTP(BaSender.Account.Email);
            if (mail == otp)
            {
                return new JsonResult(true);
            }
            return new JsonResult(false);
        }
    }
}
