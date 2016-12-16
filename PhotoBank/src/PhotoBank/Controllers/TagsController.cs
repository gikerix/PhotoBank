using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Collections.Generic;
using PhotoBank.Models;

namespace PhotoBank.Controllers
{
    public class TagsController : Controller
    {
        private PhotoBankContext db;

        public TagsController(PhotoBankContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            List<SelectList> list = new List<SelectList>();
            list.Add( new SelectList())
            ViewBag.TagList = db.Tags.Select(t => new SelectListItem(t.TagPhrase));
            return View();
        }
    }
}
