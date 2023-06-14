using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using NP.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace NP.Models
{
    public class Location
    {
        [Key]
        public string Id { get; set; }
        public string? Latitude { get; set; }
        public string? Longtitude { get; set; }
        public string? Email { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string? Img { get; set; }


        public string? AddedById { get; set; }
        public User? AddedBy { get; set; }

        public IList<Book> BooksStored { get; } = new List<Book>();

        public bool isVerified { get; set; }
    }

    
}

