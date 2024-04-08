using Microsoft.AspNetCore.Mvc;

namespace Project_OnlineBanking.Controllers
{
    public class SupportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
