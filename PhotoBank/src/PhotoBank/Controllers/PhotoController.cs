using Microsoft.AspNetCore.Mvc;
using System.Linq;
using PhotoBank.Models;
using System.IO;

namespace PhotoBank.Controllers
{
    public class PhotoController : Controller
    {
        // GET: /<controller>/
        private PhotoBankContext db;

        public PhotoController(PhotoBankContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View(db.Photos.ToList());
        }

        [HttpPost]
        public IActionResult UploadPhoto()
        {            
            var photos = Request.Form.Files;
            if (photos.Count == 0)
                return RedirectToAction("Index");
            foreach (var photo in photos)
            {
                if (photo.Length <= 0)
                    continue;               
                using (var reader = new BinaryReader(photo.OpenReadStream()))
                {
                    byte[] data = reader.ReadBytes((int)photo.Length);
                    db.Photos.Add(new Photo()
                    {
                        Data = data,
                        Name = "Default Name"
                    });
                }               
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeletePhoto(int photoIndex)
        {
            var photo = db.Photos.Where(p => p.Id == photoIndex);
            foreach (var ph in photo)
                db.Photos.Remove(ph);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
