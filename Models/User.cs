using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace NextPage.Models
{
    public class User : IdentityUser
	{
        public string Nickname { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }

    //public Book AddBook(string title, string description, string author, DateTime published, Genre genre,
    //    Difficulty difficulty, int numberOfPages, decimal price, User postedBy, DateTime postedDate, Byte img, )
}

