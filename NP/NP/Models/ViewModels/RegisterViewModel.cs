using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NP.Models.ViewModels
{
	public class RegisterViewModel
	{
        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string FirstName { get; set; }
        [StringLength(20, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 2)]
        [Key]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password not match.")]
        public string ConfirmPassword { get; set; }
    }
}

