using Microsoft.AspNetCore.Mvc;
using PagedList;
using Project_OnlineBanking.Services;

namespace Project_OnlineBanking.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {
        private UserService userService;
        private TransactionService transactionService;

        public HomeController(UserService _userService, TransactionService _transactionService)
        {
            userService = _userService;
            transactionService = _transactionService;
        }
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            int bankId = (int)HttpContext.Session.GetInt32("bankId");
            var service = userService.findByUsername(HttpContext.Session.GetString("username"));
            ViewBag.fullname = service.FullName;
            if (bankId > 0)
            {
                ViewBag.balance = userService.findByBankId(bankId).Balance;
                ViewBag.banknumber = userService.findByBankId(bankId).AccountNumber;
            }else
            {
                ViewBag.balance = 0;
                ViewBag.banknumber = null;
            }
            return View();
        }


        [Route("contact")]
        public IActionResult Contact()
        {
            return View("Contact");
        }

        [Route("transaction/{type}")]
        public IActionResult Transaction(string type, int? page, int pageSize)
        {
            int BankId = (int)HttpContext.Session.GetInt32("bankId");
            pageSize = 5;
            int pageNumber = (page ?? 1);
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
            return View("Transaction", transactions.ToPagedList(pageNumber, pageSize));
        }
    }
}
