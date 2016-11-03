using System.ComponentModel.DataAnnotations;

namespace PhotoBank.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public byte[] Data { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
