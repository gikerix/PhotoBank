using System.ComponentModel.DataAnnotations;

namespace PhotoBank.Models
{
    public class PhotoTags
    {
        public int PhotoID { get; set; }
        public Photo Photo { get; set; }
        public int TagID { get; set; }
        public Tag Tag { get; set; }
    }
}
