﻿using Microsoft.AspNetCore.Mvc;
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
            //ViewBag.TagList = db.Tags.Select(t => new SelectListItem()
            //{
            //    Text = t.TagPhrase,
            //    Value = t.TagID.ToString(),
            //});
            return View(db.Tags.ToList());
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

        public IActionResult DeleteTag(int tagIndex)
        {
            var tag = db.Tags.Where(p => p.TagID == tagIndex);
            foreach (var t in tag)
                db.Tags.Remove(t);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
