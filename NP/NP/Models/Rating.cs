using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace NP.Models
{
	public class Rating
	{
		
		public int Id;
		public string UserId { get; set; }
		public string BookId { get; set; }
		public int CategoryId { get; set; }
        public double RatingValue { get; set; }

        public virtual Book Book { get; set; }

    }
}

