using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_OnlineBanking.Services;
using System;

namespace Project_OnlineBanking.Areas.Admin.Controllers;

[Authorize(Roles = "Admin")]
[Area("admin")]
[Route("admin/support")]
public class SupportController : Controller
{
    private SupportService supportService;

    private TransactionService transactionService;
    public SupportController(SupportService _supportService, TransactionService _transactionService)
    {
        supportService = _supportService;
        transactionService = _transactionService;
    }
    [Route("list")]
    public IActionResult List()
    {
        ViewBag.supports = supportService.findAll();
        return View("List");
    }

    [Route("editStatus")]
    public IActionResult EditStatus(int id)
    {
        var support = supportService.findById(id);
        string status = support.Status;
        if (supportService.EditStatus(id, status))
        {
            TempData["msg"] = "Success";
            return RedirectToAction("list");
        }
        TempData["msg"] = "Fail";
        return RedirectToAction("list");
    }

    [Route("searchStatus")]
    public IActionResult SearchStatus(string selected)
    {
        if (selected.Equals("all"))
        {
            ViewBag.supports = supportService.findAll();
        }
        else
        {
            ViewBag.supports = supportService.searchStatus(selected);
        }

        return View("List");
    }


}
