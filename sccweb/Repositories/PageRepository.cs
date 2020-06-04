using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using sccweb.Models;
using sccweb.ViewModel;

namespace sccweb.Repositories
{
    public class PageRepository
    {
        private readonly sccwebEnt db = new sccwebEnt();
        public int UploadImageInDataBase(HttpPostedFileBase file, PageViewModel PageViewModel)
        {
            PageViewModel.Img = ConvertToBytes(file);
            
            var page = new Page
            {
                Title = PageViewModel.Title,
                Summary = PageViewModel.Summary,
                Maintext = PageViewModel.Maintext,
                Created = PageViewModel.Created,
                MetaDescription = PageViewModel.MetaDescription,
                MetaKeywords = PageViewModel.MetaKeywords,
                Publish = PageViewModel.Publish,
                AuthorId = PageViewModel.AuthorId,
                ImageId = PageViewModel.ImageId,
                ParentId = PageViewModel.ParentId,
                NavgroupId = PageViewModel.NavgroupId,
                Img = PageViewModel.Img,
                SubContent = PageViewModel.SubContent,
                PageUrl = PageViewModel.PageUrl
            };
            db.Pages.Add(page);
            int i = db.SaveChanges();
            if (i == 1)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }

        public byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}