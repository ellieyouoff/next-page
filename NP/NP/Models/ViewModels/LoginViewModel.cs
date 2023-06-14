using System;
using System.ComponentModel.DataAnnotations;

namespace NP.Models.ViewModels
{
	public class LoginViewModel
	{
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}

