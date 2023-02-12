using System;
using System.ComponentModel.DataAnnotations;

namespace NextPage.Models.ViewModels
{
	public class Login
	{

		[Required]
        [DataType(DataType.Text)]
        public string Nickname { get; set; }
        [Required]
		[DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
	}
}

