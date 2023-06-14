using System.Diagnostics;
using NP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NP.Data;
using Microsoft.EntityFrameworkCore;

namespace NP.Controllers;

public class HomeController : Controller
{
    private readonly NPDbContext context;
    private readonly ILogger<HomeController> logger;

    public HomeController(NPDbContext context, ILogger<HomeController> logger)
    {
        this.context = context;
        this.logger = logger;
    }
    

    public async Task<IActionResult> Index()
    {
        var booksList = await context.Books.ToListAsync();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult Logout()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

