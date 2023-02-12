using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextPage.Models
{
	public class Book
	{
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Author { get; set; }
        [Column(TypeName = "Date")]
        public DateTime Published { get; set; }
        public Genre Genre { get; set; }
        public Difficulty Difficulty { get; set; }
        public int NumberOfPages { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Price { get; set; }
        public string ISBN { get; set; }
        public User PostedBy { get; set; }
        public DateTime PostedDate { get; set; }
        public System.Byte[]? Img { get; set; }
        public bool Held { get; set; }
        public Guid StoredAt { get; set; }
        public User HeldBy { get; set; }
        public string UrlHandle { get; set; }
    }
}

