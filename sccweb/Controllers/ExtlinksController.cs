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

namespace sccweb.Controllers
{
    [RoutePrefix("Extlinks")]
    [ValidateInput(false)]
    public class ExtlinksController : Controller
    {
        private readonly sccwebEnt db = new sccwebEnt();
        // GET: Extlinks
        [Route("Index")]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.ExtlinkPanelActive = "active";

            var Extlink = db.Extlinks.Select(s => new
            {
                s.Id,
                s.Title,
                s.UrlLink,
                s.Created,
                s.Author,
                s.ImageId,
                s.Parent,
                s.NavgroupId,
                s.Img
            });

            List<ExtlinkViewModel> ExtlinkModel = Extlink.Select(item => new ExtlinkViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                UrlLink = item.UrlLink,
                Created = item.Created,
                Author = item.Author,
                ImageId = item.ImageId,
                Parent = item.Parent,
                NavgroupId = item.NavgroupId,
                Img = item.Img
            }).ToList();
            return View(ExtlinkModel);
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

        public byte[] GetImageFromDataBase(int Id)
        {
            var q = from temp in db.Extlinks where temp.Id == Id select temp.Img;
            byte[] cover = q.First();
            return cover;
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.ExtlinkPanelActive = "active";
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public ActionResult Create(ExtlinkViewModel model)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.ExtlinkPanelActive = "active";
            HttpPostedFileBase file = Request.Files["upload"];
            ExtlinkRepository service = new ExtlinkRepository();
            int i = service.UploadImageInDataBase(file, model);
            if (i == 1)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}