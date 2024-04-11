using Microsoft.AspNetCore.Mvc;
using Project_OnlineBanking.Services;
using System.Diagnostics;

namespace Project_OnlineBanking.Controllers
{
    [Route("transaction")]
    public class TransactionController : Controller
    {
        private TransactionService transactionService;

        public TransactionController(TransactionService _transactionService)
        {
            transactionService = _transactionService;
        }
        [Route("index/{type}")]
        public IActionResult Index(string type, int num)
        {
            if (num == 0)
            {
                num = 5;
            }
            int BankId = (int)HttpContext.Session.GetInt32("bankId");
            ViewBag.type = type;
            ViewBag.bankId = BankId;
            var transactions = transactionService.findByAccountId(BankId);
            if (type == "Transfer")
            {
                transactions = transactionService.findByTypeTrans(BankId, "Banking");
            }
            else if (type == "Receive")
            {
                transactions = transactionService.findByTypeRec(BankId, "Banking");
            }
            else if (type == "Recharge")
            {
                transactions = transactionService.findByTypeRec(BankId, "Recharge");
            }
            ViewBag.transactions = transactions.Take(num);
            @TempData["num"] = num;
            return View("Index");
        }
    }
}
