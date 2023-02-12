using System;
using System.ComponentModel.DataAnnotations;

namespace NextPage.Models.ViewModels
{
	public class Registration
	{
        [Required]
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match!")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}

