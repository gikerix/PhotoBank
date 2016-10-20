using System.ComponentModel.DataAnnotations;

namespace PhotoBank.Models
{
    public class Photo
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public byte[] Data { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
