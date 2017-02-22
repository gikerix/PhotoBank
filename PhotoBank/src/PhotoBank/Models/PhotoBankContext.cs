using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace PhotoBank.Models
{
    public class PhotoBankContext : IdentityDbContext<User>
    {
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<PhotoTags> PhotoTags { get; set; }

        public PhotoBankContext(DbContextOptions<PhotoBankContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PhotoTags>().HasKey(pt => new { pt.PhotoID, pt.TagID });
            modelBuilder.Entity<PhotoTags>().HasOne(pt => pt.Photo).WithMany(p => p.PhotoTags).HasForeignKey(pt => pt.PhotoID);
            modelBuilder.Entity<PhotoTags>().HasOne(pt => pt.Tag).WithMany(p => p.PhotoTags).HasForeignKey(pt => pt.TagID);
        }
    }
}
