using System;
using NextPage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NextPage.Data
{
	public class NextPageDbContext : IdentityDbContext<User>
	{
		public NextPageDbContext(DbContextOptions<NextPageDbContext> options) : base(options)
		{
		}

		public DbSet<Book> Books { get; set; }
		public DbSet<Location> Locations { get; set; }
		public DbSet<Genre> Genres { get; set; }
        public override DbSet<User> Users { get; set; }

    }
}

