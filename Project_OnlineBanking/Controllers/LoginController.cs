using Microsoft.AspNetCore.Mvc;

namespace Project_OnlineBanking.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {

        [Route("index")]
        [Route("")]
        [Route("~/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
