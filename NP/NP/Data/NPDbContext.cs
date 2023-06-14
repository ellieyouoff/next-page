using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NP.Models;
using Microsoft.EntityFrameworkCore.Design;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using Microsoft.ML;
using NP.Data;



namespace NP.Data
{


    public class NPDbContext : IdentityDbContext<User>
    {

        private readonly DbContextOptions _options;
        public NPDbContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasKey(b => b.BookId);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Book>()
                .HasOne(p => p.PostedBy)
                .WithMany(b => b.BooksPosted)
                .HasForeignKey(p => p.PostedById)
                .HasPrincipalKey(p => p.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Book>()
                .HasOne(l => l.CurrentlyStoredAt)
                .WithMany(b => b.BooksStored)
                .HasForeignKey(l => l.CurrentlyStoredAtId)
                .HasPrincipalKey(l => l.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Book>()
                .HasOne(l => l.CurrentlyHeldBy)
                .WithMany(b => b.BooksStored)
                .HasForeignKey(l => l.CurrentlyHeldById)
                .HasPrincipalKey(l => l.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rating>()
                .HasKey(r => r.Id);

        }

        public override DbSet<User> Users { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Book> Books { get; set; }

        public DbSet<Rating> Ratings { get; set; }



    }

}

