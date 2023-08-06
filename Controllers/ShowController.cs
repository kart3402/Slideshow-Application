using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Slideshow.Models;
using System.Data.Entity;
using System.IO;

namespace Slideshow.Controllers
{
    public class ShowController : Controller
    {
        SlideingEntities sl=new SlideingEntities();
       
        public ActionResult Input()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Input(slide pa, HttpPostedFileBase[] photo)
        {

            if (!(ModelState.IsValid))
            {
                return View();
            }
            else
            {
                string imagesnames = " ";
                string folder = Server.MapPath("~/Photos/" + pa.slide_name);
                int i = 1;
                foreach (HttpPostedFileBase h in photo)
                {
                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }
                    string imgname = pa.slide_name + i + Path.GetExtension(h.FileName);
                    string imgpath = folder + "/" + imgname;
                    h.SaveAs(imgpath);
                    i++;
                    imagesnames = imagesnames + "," + imgname;
                }
                pa.slide_clips = imagesnames;
                sl.slides.Add(pa);
                sl.SaveChanges();
                return RedirectToAction("Output");
            }
        }


        public ActionResult Edit( int id)
        {
            slide p = sl.slides.Find(id);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(slide s)
        {
            if(!(ModelState.IsValid))
            {
                return View();
            }
            else
            {
                sl.Entry<slide>(s).State=EntityState.Modified;
                sl.SaveChanges();
                return RedirectToAction("Output");
            }
        }
        public ActionResult Remove(int id)
        {
            if (!(ModelState.IsValid))
            {
                return View();
            }
            else
            {
                slide s= sl.slides.Find(id);
                sl.slides.Remove(s);
                sl.SaveChanges();
                return RedirectToAction("Output");

            }
        }

        public ActionResult Output()
        {
           List<slide>tt=sl.slides.ToList();
            return View(tt);
        }
    }
}