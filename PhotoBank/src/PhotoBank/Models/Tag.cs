using System.ComponentModel.DataAnnotations;

namespace PhotoBank.Models
{
    public class Tag
    {
        [Key]
        public int TagID { get; set; }
        [Required]
        public string TagPhrase { get; set; }
    }
}
