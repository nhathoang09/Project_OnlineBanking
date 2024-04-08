using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_OnlineBanking.Models;
using Project_OnlineBanking.Services;

namespace Project_OnlineBanking.Areas.Admin.Controllers;
[Authorize(Roles = "Admin")]
[Area("admin")]
[Route("admin/owner")]
public class OwnerController : Controller
{
    private AccountService accountService;

    private RoleService roleService;

    public OwnerController(AccountService _accountService, RoleService _roleService)
    {
        accountService = _accountService;
        roleService = _roleService;
    }
    
    [Route("index")]
    public IActionResult Index()
    {   
        ViewBag.accounts = accountService.findAll();
        return View();
    }

    [Route("dashboard")]
    public IActionResult Dashboard()
    {
        return View("Dashboard");
    }

    [Route("delete")]
    public IActionResult Delete(int id)
    {
        accountService.Delete(id); 
        return RedirectToAction("index");
    }

    [Route("editStatus")]
    public IActionResult EditStatus(int id)
    {
        var account = accountService.findById(id);
        bool status = (bool)account.IsTransferEnabled;
        if (accountService.EditStatus(id, status))
        {
            TempData["msg"] = "Success";
            return RedirectToAction("index");
        }
        TempData["msg"] = "Fail";
        return RedirectToAction("index");
    }

    [Route("detail/{id}")]
    public IActionResult Detail(int id)
    {
        var account = accountService.findById(id);
        ViewBag.account_last = account;
        return View("Detail", account);
    }

    [HttpPost]
    [Route("detail/{AccountId}")]
    public IActionResult Detail(Account account, string password)
    {
        if (password != null)
        {
            account.Password = BCrypt.Net.BCrypt.HashPassword(password);
        }
        if (accountService.update(account))
        {
            return RedirectToAction("index");
        }
        return RedirectToAction("detail", new { id = account.AccountId });
    }

    [Route("searchStatus")]
    public IActionResult SearchStatus(string selected)
    {
        if (selected.Equals("all"))
        {
            ViewBag.accounts = accountService.findAll();
        }
        else
        {
           var status = (selected.Equals("true")) ? true: false;
           ViewBag.accounts = accountService.searchStatus(status);
        }
        
        return View("Index");
    }
}
