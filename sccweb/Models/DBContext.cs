﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace sccweb.Models
{
    public class sccwebEnt : DbContext
    {
        public sccwebEnt()
            : base("name=sccwebEnt")
        {
            //Database.SetInitializer<sccwebEnt>(new DropCreateDatabaseIfModelChanges<sccwebEnt>());
        }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Pdf> Pdfs { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Extlink> Extlinks { get; set; }
        public DbSet<Menugroup> Menugroups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Modal> Modals { get; set; }
    }
}