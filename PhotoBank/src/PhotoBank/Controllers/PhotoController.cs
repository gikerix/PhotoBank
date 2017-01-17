using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;

using PhotoBank.Models;
using PhotoBank.ViewModels;
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
            TagsPhotoIndexViewModel viewModel = new TagsPhotoIndexViewModel();
            viewModel.TagSelectionList = db.Tags.Select(t => new SelectListItem()
                                                            {
                                                                Text = t.TagPhrase,
                                                                Value = t.TagID.ToString(),
                                                            });
            var photoTags = db.PhotoTags.Include(pt => pt.Tag).Include(pt => pt.Photo);
            var photos = db.Photos.Include(p => p.PhotoTags);
            foreach (var photo in photos)
            {
                photo.PhotoTags = photoTags.Where(pt => pt.PhotoID == photo.PhotoID).ToList();
            }
            viewModel.Photos = photos;            
            return View("PhotoIndex", viewModel);
        }

        [HttpPost]
        public IActionResult UploadPhoto(string photoName)
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
                        Name = string.IsNullOrEmpty(photoName.Trim()) ? "Default Name" : photoName,
                        FileExtention = System.IO.Path.GetExtension(photo.FileName),
                    });
                }               
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeletePhoto(int photoID)
        {
            var photo = db.Photos.Where(p => p.PhotoID == photoID);
            foreach (var ph in photo)
                db.Photos.Remove(ph);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult AttachTagToPhoto(int tagID, int photoID)
        {
            var tag = db.Tags.Where(t => t.TagID == tagID);
            if (tag.Count() != 1)
                return RedirectToAction("Index");
            var photo = db.Photos.Where(p => p.PhotoID == photoID).Include(p => p.PhotoTags);
            if (photo.Count() != 1)
                return RedirectToAction("Index");
            Photo ph = photo.FirstOrDefault();
            if (ph.PhotoTags.Where(pt=> pt.TagID == tagID).Count() > 0)
                return RedirectToAction("Index");
            ph.PhotoTags.Add(new PhotoTags { PhotoID = photoID, TagID = tagID });
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public FileResult DownloadPhoto(int photoID)
        {
            var photo = db.Photos.Where(p => p.PhotoID == photoID);
            if (photo.Count() != 1)
                RedirectToAction("Index");
            Photo ph = photo.FirstOrDefault();
            string extention = string.IsNullOrEmpty(ph.FileExtention) ? "unknown" : ph.FileExtention;
            return File(ph.Data, string.Format("applocation/{0}", extention), string.Format("{0}.{1}", ph.Name, extention));
        }
    }
}
