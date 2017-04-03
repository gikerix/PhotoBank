using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PhotoBank.Models;
using Microsoft.AspNetCore.Identity;

namespace PhotoBank.Controllers
{
    public class TagsController : Controller
    {
        private PhotoBankContext db;
        private UserManager<User> userManager;

        public TagsController(PhotoBankContext Context, UserManager<User> UserManager)
        {
            db = Context;
            userManager = UserManager;
        }

        public IActionResult Index()
        {            
            return View("TagIndex", db.Tags.ToList());
        }

        [HttpPost]
        public IActionResult CreateNewTag(Tag tag)            
        {
            if (string.IsNullOrEmpty(tag.TagPhrase))
                return RedirectToAction("Index");
            db.Tags.Add(tag);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteTag(int tagID)
        {
            var tag = db.Tags.Where(p => p.TagID == tagID);
            foreach (var t in tag)
                db.Tags.Remove(t);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult TagPhotos(int tagID)
        {
            string userID = userManager.GetUserId(HttpContext.User);
            return View(new ViewModels.TagsPhotoIndexViewModel()
                        {
                            Photos = db.PhotoTags.Include(pt => pt.Photo).
                                                  Where(pt => pt.TagID == tagID && (pt.Photo.UploadedByUserID == userID || pt.Photo.UploadedByUserID == null)).
                                                  Select(pt => pt.Photo).ToList()
                        });
        }
    }
}
