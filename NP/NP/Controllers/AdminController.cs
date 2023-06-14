using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace NP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
	{

            public IActionResult Index() =>
                Content("Admin");
    }
}

