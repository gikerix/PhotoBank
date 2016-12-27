using PhotoBank.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PhotoBank.ViewModels
{
    public class TagsPhotoIndexViewModel
    {
        public IEnumerable<Photo> Photos { get; set; }
        public IEnumerable<SelectListItem> TagSelectionList { get; set; }
    }
}
