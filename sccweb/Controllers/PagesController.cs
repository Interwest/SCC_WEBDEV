using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure;
using sccweb.Models;
using sccweb.Repositories;
using sccweb.ViewModel;
using System.Web.UI;
using System.Text.RegularExpressions;

namespace sccweb.Controllers
{
    [RoutePrefix("Pages")]
    [ValidateInput(false)]
    public class PagesController : Controller
    {
        
        
        // GET: Pages

        private readonly sccwebEnt db = new sccwebEnt();
        /// <summary>
        /// Retrive content from database 
        /// </summary>
        /// <returns></returns>
        /// 

        [Route("Index")]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PagePanelActive = "active";

            var Page = db.Pages.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.Maintext,
                s.Created,
                s.MetaDescription,
                s.MetaKeywords,
                s.Publish,
                s.AuthorId,
                s.ImageId,
                s.ParentId,
                s.NavgroupId,
                s.Img,
                s.SubContent
            });

            List<PageViewModel> pageModel = Page.Select(item => new PageViewModel()
            {
                
                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Maintext = item.Maintext,
                Created = item.Created,
                MetaDescription = item.MetaDescription,
                MetaKeywords = item.MetaKeywords,
                Publish = item.Publish,
                AuthorId = item.AuthorId,
                ImageId = item.ImageId,
                ParentId = item.ParentId,
                NavgroupId = item.NavgroupId,
                Img = item.Img,
                SubContent = item.SubContent
            }).ToList();
            return View(pageModel);
        }

        public ActionResult RetrieveImage(int id)
        {
            byte[] cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public byte[] GetImageFromDataBase(int Id)
        {
            var q = from temp in db.Pages where temp.Id == Id select temp.Img;
            byte[] cover = q.First();
            return cover;
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PagePanelActive = "active";
            return View();
        }
        /// <summary>
        /// Save content and images
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Create")]
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(PageViewModel model)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PagePanelActive = "active";
            HttpPostedFileBase file = Request.Files["upload"];
            PageRepository service = new PageRepository();

            List<string> modes = new List<string>();
            string v = Request["subcontent"];
            string[] values = v.Split(',');
            for (int vi = 0; vi < values.Length; vi++)
            {
                modes.Add(values[vi].Trim());
            }
            
            //ViewBag.Subcontent = modes;

            model.SubContent = Request["subcontent"];

            int i = service.UploadImageInDataBase(file, model);
            if (i == 1)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }


        [Route("Detail")]
        [HttpGet]

        public ActionResult Detail(int? id, string pageName)
        {
            ViewBag.Imgbg = true;
            ViewBag.Class = "page-item";
            ViewBag.PagePanelActive = "active";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var PagesView = db.Pages.Find(id);
            if (PagesView == null)
            {
                return HttpNotFound();
            }

            if (PagesView.Summary != null)
            {
                ViewBag.Summary = PagesView.Summary;
            }
            else
            {
                ViewBag.Summary = "";
            }

            if (PagesView.Img != null)
            {
                ViewBag.Img = true;
            }
            else
            {
                ViewBag.Img = false;
            }

            List<string> modes = new List<string>();
            string v = PagesView.SubContent.ToString();
            string[] values = v.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                modes.Add(values[i].Trim());
            }
            ViewBag.Subcontent = modes;

            return View(PagesView);
        }

        
        public ActionResult Edit(int? id)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PagePanelActive = "active";
            ViewBag.Img = false;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Page = db.Pages.Find(id);
            if (Page == null)
            {
                return HttpNotFound();
            }

            if (Page.Img != null) {
                ViewBag.Img = true;
            }

            List<string> modes = new List<string>();
            string v = Page.SubContent.ToString();
            string[] values = v.Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                modes.Add(values[i].Trim());
            }
            ViewBag.Subcontent = modes;

            return View(Page);
        }

        [HttpPost, ActionName("Edit")]
        public ActionResult EditPost(int? id, PageViewModel model)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PagePanelActive = "active";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Page = db.Pages.Find(id);
            if (Page == null)
            {
                return HttpNotFound();
            }

            Page.SubContent = Request["subcontent"];

            if (Request.Files["upload"] != null)
            {
                HttpPostedFileBase file = Request.Files["upload"];

                Page.Img = ConvertToBytesEdit(file);

                if (TryUpdateModel(Page, "", new string[] { "Title", "Summary", "Maintext", "Created", "MetaDescription", "MetaKeywords", "Publish", "AuthorId", "ImageId", "ParentId", "NavgroupId", "Img"}));
            }
            else 
            {
                if (TryUpdateModel(Page, "", new string[] { "Title", "Summary", "Maintext", "Created", "MetaDescription", "MetaKeywords", "Publish", "AuthorId", "ImageId", "ParentId", "NavgroupId" }));
            }

            db.Entry(Page).State = EntityState.Modified;
            int i = db.SaveChanges();

            byte[] ConvertToBytesEdit(HttpPostedFileBase image)
            {
                byte[] imageBytes = null;
                var reader = new System.IO.BinaryReader(image.InputStream);
                imageBytes = reader.ReadBytes((int)image.ContentLength);
                return imageBytes;
            }

            Console.WriteLine("<script>alert("+ Page.SubContent + ");</script>");

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int? id)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PagePanelActive = "active";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Page = db.Pages.Find(id);
            if (Page == null)
            {
                return HttpNotFound();
            }

            return View(Page);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePost(int id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Page = db.Pages.Find(id);
            if (Page == null)
            {
                return HttpNotFound();
            }

            db.Pages.Remove(Page);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}

