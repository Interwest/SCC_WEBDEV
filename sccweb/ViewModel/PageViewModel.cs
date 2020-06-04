using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sccweb.ViewModel
{
    public class PageViewModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        [AllowHtml]
        public string Maintext { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeywords { get; set; }
        public Nullable<int> Publish { get; set; }
        public Nullable<int> AuthorId { get; set; }
        public Nullable<int> ImageId { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<int> NavgroupId { get; set; }
        public byte[] Img { get; set; }
        [AllowHtml]
        public string SubContent { get; set; }
        public string PageUrl { get; set; }
    }
}