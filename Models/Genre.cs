using System;
namespace NextPage.Models
{
	public class Genre
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public Guid BookId { get; set; }

	}
}

