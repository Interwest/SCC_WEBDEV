using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using sccweb.Models;
using sccweb.ViewModel;

namespace sccweb.Repositories
{
    public class ExtlinkRepository
    {
        private readonly sccwebEnt db = new sccwebEnt();
        public int UploadImageInDataBase(HttpPostedFileBase file,ExtlinkViewModel ExtlinkViewModel)
        {
            ExtlinkViewModel.Img = ConvertToBytes(file);
            var extlink = new Extlink
            {
                Title = ExtlinkViewModel.Title,
                UrlLink = ExtlinkViewModel.UrlLink,
                Created = ExtlinkViewModel.Created,
                Author = ExtlinkViewModel.Author,
                ImageId = ExtlinkViewModel.ImageId,
                NavbarId = ExtlinkViewModel.NavbarId,
                Img = ExtlinkViewModel.Img,
                IsExternal = ExtlinkViewModel.IsExternal
            };
            db.Extlinks.Add(extlink);
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