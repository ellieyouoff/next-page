using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NP.Data;
using NP.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NP.Controllers
{
    public class CabinetController : Controller
    {

        private readonly NPDbContext _context;
        private readonly UserManager<User> _userManager;

        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        public CabinetController(NPDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            User usr = await GetCurrentUserAsync();
            var booksList = from b in _context.Books where b.CurrentlyHeldById == usr.Id select b;

            return View(booksList);
        }

        public async Task<IActionResult> CurrentlyInPosession()
        {
            User usr = await GetCurrentUserAsync();
            var booksList = from b in _context.Books where b.CurrentlyHeldById == usr.Id select b;

            return View(booksList);
        }



        public async Task<IActionResult> PostedByMe()
        {
            User usr = await GetCurrentUserAsync();
            var booksList = from b in _context.Books where b.PostedById == usr.Id select b ;

            return View(booksList);
        }

    }
}

