using Microsoft.AspNetCore.Mvc;

namespace Project_OnlineBanking.Controllers
{
    [Route("contact")]
    public class ContactController : Controller
    {
        [Route("index")]
        public IActionResult Contact()
        {
            return View("index");
        }
    }
}
