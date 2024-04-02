using Microsoft.AspNetCore.Mvc;
using Project_OnlineBanking.Models;
using Project_OnlineBanking.Services;
using System.Diagnostics;

namespace Project_OnlineBanking.Controllers
{
    [Route("login")]
    public class LoginController : Controller
    {
        private UserService userService;

        public LoginController(UserService _userService)
        {
            userService = _userService;
        }

        [Route("login")]
        [Route("")]
        [Route("~/")]
        public IActionResult Login([FromBody] Account account)
        {
            try
            {
                return Ok(new
                {
                    Results = userService.Login(account.PhoneNumber, account.Password)
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return BadRequest();
            }
        }
    }
}
