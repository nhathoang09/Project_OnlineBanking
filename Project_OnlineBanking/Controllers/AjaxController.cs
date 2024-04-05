using Microsoft.AspNetCore.Mvc;
using Project_OnlineBanking.Services;

namespace Project_OnlineBanking.Controllers
{
    [Route("ajax")]
    public class AjaxController : Controller
    {
        private UserService userService;
        public AjaxController(UserService _userService)
        {
            userService = _userService;
        }

        [Route("fullnameByAccnum")]
        public IActionResult FullnameByAccountNumber(string accnum)
        {
            return new JsonResult(userService.findFullnameByAccnum(accnum));
        }
    }
}
