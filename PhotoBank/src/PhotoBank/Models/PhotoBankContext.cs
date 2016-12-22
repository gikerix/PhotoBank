using Microsoft.EntityFrameworkCore;

namespace PhotoBank.Models
{
    public class PhotoBankContext : DbContext
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
            modelBuilder.Entity<PhotoTags>().HasKey(pt => new { pt.PhotoID, pt.TagID });
            modelBuilder.Entity<PhotoTags>().HasOne(pt => pt.Photo).WithMany(p => p.PhotoTags).HasForeignKey(pt => pt.PhotoID);
            modelBuilder.Entity<PhotoTags>().HasOne(pt => pt.Tag).WithMany(p => p.PhotoTags).HasForeignKey(pt => pt.TagID);
        }
    }
}
