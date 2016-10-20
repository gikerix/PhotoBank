using System.ComponentModel.DataAnnotations;

namespace PhotoBank.Models
{
    public class PhotoTags
    {
        [Required]
        int PhotoID { get; set; }
        [Required]
        int TagID { get; set; }        
    }
}
