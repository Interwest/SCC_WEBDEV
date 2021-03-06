﻿using System;
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
using PagedList.Mvc;
using PagedList;

namespace sccweb.Controllers
{
    public class HomeController : Controller
    {
        private readonly sccwebEnt db = new sccwebEnt();

        public ActionResult Index()
        {
            ViewBag.Footer = true;

            var MenuItems = db.Menugroups.Select(s => new
            {
                s.Id,
                s.Name,
                s.PageId,
                s.ParentId,
                s.IsParent,
                s.Type,
                s.ExtlinkId,
                s.PdfId,
                s.ModalId,
                s.SortOrder
            }).OrderBy(s => s.SortOrder);

            List<MenugroupViewModel> SubMenuLists = MenuItems.Select(item => new MenugroupViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                PageId = item.PageId,
                ParentId = item.ParentId,
                IsParent = item.IsParent,
                Type = item.Type,
                ExtlinkId = item.ExtlinkId,
                PdfId = item.PdfId,
                ModalId = item.ModalId
            }).ToList();

            var PdfItems = db.Pdfs.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.Publish,
                s.Img,
                s.FileName,
                s.NavbarId,
                s.IsExternal,
                s.ExLink
            }).Where(s => s.Publish == 1);

            List<PdfViewModel> PdfItemLists = PdfItems.Select(item => new PdfViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Publish = item.Publish,
                Img = item.Img,
                FileName = item.FileName,
                NavbarId = item.NavbarId,
                IsExternal = item.IsExternal,
                ExLink = item.ExLink
            }).ToList();

            var ExLinkItems = db.Extlinks.Select(s => new
            {
                s.Id,
                s.Title,
                s.UrlLink,
                s.Img,
                s.NavbarId,
                s.IsExternal,
                s.Icon
            });

            List<ExtlinkViewModel> ExLinkLists = ExLinkItems.Select(item => new ExtlinkViewModel()
            {
                Id = item.Id,
                Title = item.Title,
                UrlLink = item.UrlLink,
                Img = item.Img,
                NavbarId = item.NavbarId,
                IsExternal = item.IsExternal,
                Icon = item.Icon
            }).ToList();

            var Page = db.Pages.Select(s => new
            {
                s.Id,
                s.Title,
                s.Summary,
                s.Publish,
                s.ImageId,
                s.Img,
            }).Where(s => s.Publish == 1);

            List<PageViewModel> PageLists = Page.Select(item => new PageViewModel()
            {

                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Publish = item.Publish,
                ImageId = item.ImageId,
                Img = item.Img
            }).ToList();

            var Modal = db.Modals.Select(s => new
            { 
                s.Id,
                s.Title,
                s.Summary,
                s.Maintext,
                s.NavbarId,
                s.Img,
                s.Icon
            });

            List<ModalViewModel> ModalLists = Modal.Select(item => new ModalViewModel()
            { 
                Id = item.Id,
                Title = item.Title,
                Summary = item.Summary,
                Maintext = item.Maintext,
                NavbarId = item.NavbarId,
                Img = item.Img,
                Icon = item.Icon
            }).ToList();

            var MenuFooter = db.Menugroups.Select(s => new
            {
                s.Id,
                s.Name,
                s.PageId,
                s.ParentId,
                s.IsParent,
                s.Type,
                s.ExtlinkId,
                s.PdfId,
                s.ModalId,
                s.SortOrder
            }).Where(s => s.Name == "Footer");

            List<MenugroupViewModel> MenuItemFooter = MenuFooter.Select(item => new MenugroupViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                PageId = item.PageId,
                ParentId = item.ParentId,
                IsParent = item.IsParent,
                Type = item.Type,
                ExtlinkId = item.ExtlinkId,
                PdfId = item.PdfId,
                ModalId = item.ModalId
            }).ToList();

            int FooterParentId = 0;

            foreach (var FooterParent in MenuItemFooter) {
                FooterParentId = Convert.ToInt32(FooterParent.Id);
            }

            var FooterItems = db.Menugroups.Select(s => new
            {
                s.Id,
                s.Name,
                s.PageId,
                s.ParentId,
                s.IsParent,
                s.Type,
                s.ExtlinkId,
                s.PdfId,
                s.ModalId,
                s.SortOrder
            }).Where(s => s.ParentId == FooterParentId).OrderBy(s => s.SortOrder);

            List<MenugroupViewModel> MenuFooterItems = FooterItems.Select(item => new MenugroupViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                PageId = item.PageId,
                ParentId = item.ParentId,
                IsParent = item.IsParent,
                Type = item.Type,
                ExtlinkId = item.ExtlinkId,
                PdfId = item.PdfId,
                ModalId = item.ModalId
            }).ToList();

            ViewBag.MenuFooter = MenuItemFooter;
            ViewBag.MenuFooterItems = MenuFooterItems;
            ViewBag.ModalItems = ModalLists;
            ViewBag.SubMenus = SubMenuLists;
            ViewBag.PdfItems = PdfItemLists;
            ViewBag.ExLinkItems = ExLinkLists;
            ViewBag.PageItems = PageLists;
            

            foreach (var ModalPanel in ModalLists) {
                if (ModalPanel.Maintext != null) {
                    List<string> modes = new List<string>();
                    string v = ModalPanel.Maintext.ToString();
                    string[] values = v.Split(',');
                    for (int i = 0; i < values.Length; i++)
                    {
                        modes.Add(values[i].Trim());
                    }
                    ViewBag.ModalPanel = modes;
                }
            }
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}