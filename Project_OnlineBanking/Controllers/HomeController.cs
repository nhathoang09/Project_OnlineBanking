using Microsoft.AspNetCore.Mvc;
using Project_OnlineBanking.Services;

namespace Project_OnlineBanking.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {
        private UserService userService;

        public HomeController(UserService _userService)
        {
            userService = _userService;
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
    }
}
