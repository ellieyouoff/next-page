using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using NP.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace NP.Models
{
    public class User : IdentityUser
    {

        [Key]
        public override string Id { get; set; }
        public override string UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool isLocation { get; set; }


        public IList<Book> BooksStored { get; } = new List<Book>();
        public IList<Book> BooksPosted { get; } = new List<Book>();


    }

}

