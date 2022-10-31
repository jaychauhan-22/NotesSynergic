using KeepNotes.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeApp.Modals
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Note>()
            //    .HasOne<Category>(n => n.Category)
            //    .WithMany()
            //    .HasForeignKey(c => c.CategoryId)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

            //modelBuilder.Entity<Note>()
            //    .HasOne<Users>(n => n.User)
            //    .WithMany()
            //    .HasForeignKey(c => c.UserId)
            //    .OnDelete(DeleteBehavior.ClientSetNull);

        }

        public DbSet<Users> Users{ get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Note> Note { get; set; }
    }
}
