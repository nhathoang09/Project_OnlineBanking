using Microsoft.AspNetCore.Mvc;

namespace Project_OnlineBanking.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View("404");
        }
    }
}
