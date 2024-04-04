using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project_OnlineBanking.Models;
using Project_OnlineBanking.Services;

namespace Project_OnlineBanking.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private UserService userService;

        public AccountController(UserService _userService)
        {
            userService = _userService;
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
            if (userService.Login(username, password))
            {
                HttpContext.Session.SetString("username", username);
                return RedirectToAction("Middle");
            }
            else
            {
                TempData["msg"] = "Invalid!";
                return RedirectToAction("Login");
            }
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
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            account.RoleId = 2;
            account.IsTransferEnabled = false;
            if (userService.Create(account))
            {
                return RedirectToAction("Login");
            }
            else
            {
                TempData["msg"] = "Failed!";
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
    }
}
