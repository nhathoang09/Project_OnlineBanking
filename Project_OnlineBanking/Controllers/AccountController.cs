using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_OnlineBanking.Models;
using Project_OnlineBanking.Services;
using System.Diagnostics;
using System.Security.Claims;

namespace Project_OnlineBanking.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private UserService userService;
        private TransactionService transactionService;
        private AccountService accountService;
        private RoleService roleService;
        private DatabaseContext db;

        public AccountController(UserService _userService, TransactionService _transactionService, DatabaseContext _db,AccountService _accountService, RoleService _roleService)
        {
            userService = _userService;
            transactionService = _transactionService;
            db = _db;
            roleService = _roleService;
            accountService = _accountService;
        }

        [Route("~/")]
        [Route("")]
        [Route("login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [Route("adminRegister")]
        public IActionResult AdminRegsiter()
        {
            Account account = new Account();
            return View("Register", account);
        }

        [HttpPost]
        [Route("adminRegister")]
        public IActionResult AdminRegsiter(Account account, string password, string retrypassword)
        {
            if (password == retrypassword)
            {
                var role = roleService.findByName("Employee");
                var date = DateTime.Now;
                account.Password = BCrypt.Net.BCrypt.HashPassword(password);
                account.FullName = "";
                account.PhoneNumber = "";
                account.Address = "";
                account.RoleId = role.RoleId;
                account.FailedLoginCount = 0;
                account.LastLoginSuccess = date;
                account.FullName = "";
                account.DateOfBirth = DateOnly.FromDateTime(date);
                account.IsTransferEnabled = true;
                if (accountService.create(account))
                {
                    TempData["msg"] = "Success";
                    return RedirectToAction("login");
                }
                else
                {
                    TempData["msg"] = "Fails";
                    return RedirectToAction("register");
                }
            }
            else
            {
                return RedirectToAction("login");
            }
        }

        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            return View("Dashboard");
        }

        [Route("accessDenied")]
        public IActionResult AccessDenied()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync(string username, string password)
        {
            var account = userService.findByUsername(username);
            try
            {
                int failedSuccessCount = account.FailedLoginCount;
                HttpContext.Session.SetInt32("fsc", failedSuccessCount);
                if (failedSuccessCount < 3)
                {
                    if (account.RoleId == 1)
                    {
                        if (account != null)
                        {
                            if (userService.Login(username, password))
                            {

                                var date = DateTime.Now;
                                var claims = new List<Claim>();

                                var role = roleService.find(account.RoleId);
                                var roleName = role.Name;
                                claims.Add(new Claim(ClaimTypes.Name, username));
                                claims.Add(new Claim(ClaimTypes.Role, roleName));

                                account.LastLoginSuccess = date;

                                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                                accountService.create(account);
                                return RedirectToAction("dashboard");
                            }
                            else
                            {
                                return RedirectToAction("login");
                            }

                        }
                        else
                        {
                            return RedirectToAction("login");
                        }
                    }
                    else if (account.RoleId == 2)
                    {
                        if (userService.Login(username, password))
                        {
                            account.LastLoginSuccess = DateTime.Now;
                            account.FailedLoginCount = 0;
                            db.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            HttpContext.Session.SetString("username", username);
                            return RedirectToAction("Middle");
                        }
                        else
                        {
                            account.FailedLoginCount = failedSuccessCount + 1;
                            try
                            {
                                db.Entry(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                TempData["username"] = username;
                                TempData["msg"] = false;
                                if(db.SaveChanges() > 0)
                                {

                                    return RedirectToAction("Login");
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine(ex);
                                return RedirectToAction("Login");
                            }
                        }
                    }
                    else
                    {
                        return RedirectToAction("Login");
                    };
                }else
                {
                    TempData["lock"] = true;
                    return RedirectToAction("Login");
                }
            }catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return RedirectToAction("Login");
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            return RedirectToAction("Login");
        }

        [Route("register")]
        public IActionResult Register()
        {
            var account = new Account();
            return View("Register", account);
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(Account account)
        {
            account.LastLoginSuccess = DateTime.Now;
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            account.RoleId = 2;
            account.IsTransferEnabled = false;
            account.FailedLoginCount = 0;
            if (userService.Create(account))
            {
                TempData["username"] = account.Username;
                return RedirectToAction("Login");
            }
            else
            {
                return RedirectToAction("Register");
            }
        }

        [Route("middle")]
        public IActionResult Middle()
        {
            ViewBag.accnums = userService.findByAccId(userService.findByUsername(HttpContext.Session.GetString("username")).AccountId);
            return View("Middle");
        }

        [HttpPost]
        [Route("middle")]
        public IActionResult Middle(int bankId)
        {
            HttpContext.Session.SetInt32("bankId", bankId);
            return RedirectToAction("Index", "Home");

        }

        [Route("banknumber")]
        public IActionResult BankNumber()
        {
            var bankacc = new BankAccount();
            return View("banknumber", bankacc);
        }

        [HttpPost]
        [Route("banknumber")]
        public IActionResult BankNumber(BankAccount bankacc)
        {
            bankacc.AccountId = userService.findByUsername(HttpContext.Session.GetString("username")).AccountId;
            bankacc.Balance = 0;
            if (userService.BankNumber(bankacc))
            {
                HttpContext.Session.SetInt32("bankId", bankacc.BankAccountId);
                return RedirectToAction("index", "home");
            }
            else
            {
                TempData["msg"] = "Failed!";
                return RedirectToAction("banknumber");
            }

        }

        [HttpPost]
        [Route("transfer")]
        public IActionResult Transfer(string receiver, decimal amount, string message)
        {
            string sender = userService.findByBankId((int)HttpContext.Session.GetInt32("bankId")).AccountNumber;
            transactionService.TransferMoney(sender, receiver, amount, message);
            return RedirectToAction("index", "home");

        }
    }
}
