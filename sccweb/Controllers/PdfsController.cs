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
    [RoutePrefix("Pdfs")]
    [ValidateInput(false)]

    public class PdfsController : Controller
    {
        private readonly sccwebEnt db = new sccwebEnt();
        // GET: Pdfs
        [Route("Index")]
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PdfPanelActive = "active";

            var Pdf = db.Pdfs.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.Created,
                s.Publish,
                s.Author,
                s.ImageId,
                s.Parent,
                s.NavgroupId,
                s.Img,
                s.FileName
            });

            List<PdfViewModel> pdfModel = Pdf.Select(item => new PdfViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Created = item.Created,
                Publish = item.Publish,
                Author = item.Author,
                ImageId = item.ImageId,
                Parent = item.Parent,
                NavgroupId = item.NavgroupId,
                Img = item.Img,
                FileName = item.FileName
            }).ToList();
            return View(pdfModel);
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
            var q = from temp in db.Pdfs where temp.Id == Id select temp.Img;
            byte[] cover = q.First();
            return cover;
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PdfPanelActive = "active";
            return View();
        }

        [Route("Create")]
        [HttpPost]
        public ActionResult Create(IEnumerable<HttpPostedFileBase> file, PdfViewModel PdfViewModel)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin";
            ViewBag.PagePanelActive = "active";

            var PdfFile = file.ElementAt(1);
            var fileName = System.IO.Path.GetFileName(PdfFile.FileName);
            var path = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Content/PdfFiles/"), fileName);
            PdfFile.SaveAs(path);
            var pdf = new Pdf
            {
                Title = PdfViewModel.Title,
                Summary = PdfViewModel.Summary,
                Created = PdfViewModel.Created,
                Publish = PdfViewModel.Publish,
                Author = PdfViewModel.Author,
                ImageId = PdfViewModel.ImageId,
                Parent = PdfViewModel.Parent,
                NavgroupId = PdfViewModel.NavgroupId,
                Img = ConvertToBytes(file.ElementAt(0)),
                FileName = PdfFile.FileName
            };
            db.Pdfs.Add(pdf);
            int i = db.SaveChanges();
            if (i == 1)
            {
                return RedirectToAction("Index");
            }
            return View(PdfViewModel);


        }
        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            var reader = new System.IO.BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}