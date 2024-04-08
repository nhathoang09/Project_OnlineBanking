using Microsoft.AspNetCore.Mvc;
using Project_OnlineBanking.Models;
using Project_OnlineBanking.Services;

namespace Project_OnlineBanking.Controllers
{
    [Route("contact")]
    public class ContactController : Controller
    {
        private UserService userService;

        public ContactController(UserService _userService)
        {
            userService = _userService;
        }
        [Route("index")]
        public IActionResult Contact()
        {
            return View("index");
        }

        [HttpPost]
        [Route("ticket")]
        public IActionResult Contact(SupportTicket ticket)
        {
            ticket.AccountId = userService.findByUsername(HttpContext.Session.GetString("username")).AccountId;
            ticket.Status = "false";
            ticket.CreatedAt = DateTime.Now;
            if (userService.Ticket(ticket))
            {
                return RedirectToAction("Index", "home");
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
    }
}
