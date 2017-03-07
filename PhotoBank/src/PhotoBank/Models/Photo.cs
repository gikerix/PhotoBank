using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoBank.Models
{
    public class Photo
    {
        public Photo()
        {
            PhotoTags = new List<PhotoTags>();
        }

        [Key]
        public int PhotoID { get; set; }

        [Required]
        public byte[] Data { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string FileExtention { get; set; }

        public string UploadedByUserID { get; set; }

        public List<PhotoTags> PhotoTags {get; set; }
    }
}
