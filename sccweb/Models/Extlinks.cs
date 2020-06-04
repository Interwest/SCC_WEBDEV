﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace sccweb.Models
{
    public class Extlink
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string UrlLink { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> Author { get; set; }
        public Nullable<int> ImageId { get; set; }
        public Nullable<int> Parent { get; set; }
        public Nullable<int> NavgroupId { get; set; }
        public byte[] Img { get; set; }
    }
}