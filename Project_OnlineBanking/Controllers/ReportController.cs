using Microsoft.AspNetCore.Mvc;

namespace Project_OnlineBanking.Controllers
{
    [Route("report")]
    public class ReportController : Controller
    {
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.year = DateTime.Now.Year;
            return View();
        }
    }
}
