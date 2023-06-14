using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using NP.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace NP.Models
{
    public class Book
    {
        [Key]
        public string? BookId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }

        public double? Rating { get; set; }
        public double? CurrentRating { get; set; }

        [DefaultValue(0)]
        public int RatedCount { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Published { get; set; }
        public Genre Genre { get; set; }
        public Difficulty Difficulty { get; set; }


        public DateTime PostedDate { get; set; }

        public string? Img { get; set; }

        [NotMapped]
        public IFormFile? FrontImage { get; set; }

        public string? CurrentlyStoredAtId { get; set; }
        public Location? CurrentlyStoredAt { get; set; }

        public string? CurrentlyHeldById { get; set; }
        public User? CurrentlyHeldBy { get; set; }



        public string? PostedById { get; set; }
        public User? PostedBy { get; set; }

        public IList<User> WishlistedBy { get; } = new List<User>();

        public ICollection<Rating>? Ratings { get; set; }





    }
}

