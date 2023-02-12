using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using NextPage.Models;
using NextPage.Models.ViewModels;

namespace NextPage.Pages
{
	public class LoginModel : PageModel
    {

        private readonly SignInManager<User> signInManager;


        public LoginModel(SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
        }

        [BindProperty]
        public Login Model { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if(ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(Model.Nickname, Model.Password, Model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (returnUrl == null || returnUrl == "/")
                    {
                        return RedirectToPage("Index");
                    }
                    else
                    {
                        RedirectToPage("returnUrl");
                    }
                }
                ModelState.AddModelError("", "Username or password is incorrect.");
            }
            return Page();
        }
    }
}
