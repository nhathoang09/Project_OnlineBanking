using Microsoft.AspNetCore.Mvc;

namespace Project_OnlineBanking.Controllers
{
    [Route("contact")]
    public class ContactController : Controller
    {
        [Route("index")]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
