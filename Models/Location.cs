using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace NextPage.Models
{
	public class Location
	{
        public Guid Id { get; set; }
        [Column(TypeName = "decimal(7,5)")]
        public decimal Latitude { get; set; }
        [Column(TypeName = "decimal(7,5)")]
        public decimal Longtitude { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public System.Byte[]? Img { get; set; }
        public string UrlHandle { get; set; }
    }
}

