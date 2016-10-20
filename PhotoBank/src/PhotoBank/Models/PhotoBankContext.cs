using Microsoft.EntityFrameworkCore;

namespace PhotoBank.Models
{
    public class PhotoBankContext : DbContext
    {
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public PhotoBankContext(DbContextOptions<PhotoBankContext> options)
            : base(options)
        {
        }
    }
}
