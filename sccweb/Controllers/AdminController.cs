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
using System.Web.Security;

namespace sccweb.Controllers
{
    [RoutePrefix("Admin")]
    public class AdminController : Controller
    {
        private readonly sccwebEnt db = new sccwebEnt();

        [Route("Login")]
        public ActionResult Login() {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin admin-dashboard admin-login";
            return View();
        }

        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User objUser)
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin admin-dashboard";
            if (ModelState.IsValid)
            {
                if (ValidateUser(objUser.Username, objUser.Password))
                {
                    FormsAuthentication.SetAuthCookie(objUser.Username, false);
                    return RedirectToAction("AdminDashboard");
                }
                else
                {
                    ViewBag.Class = "admin admin-dashboard admin-login";
                    ModelState.AddModelError("", "");
                }

                return View(objUser);
            }

            return null;
        }

        private bool ValidateUser(string Username, string Password)
        {

            bool isValid = false;

            using (sccwebEnt db = new sccwebEnt())
            {
                var obj = db.Users.Where(a => a.Username.Equals(Username) && a.Password.Equals(Password)).FirstOrDefault();

                if (obj != null)
                {
                    Session["UserId"] = obj.Id;
                    Session["Username"] = obj.Username;

                    isValid = true;
                }

            }

            return isValid;
        }

        // GET: Admin
        public ActionResult AdminDashboard()
        {
            ViewBag.PageEdit = true;
            ViewBag.Class = "admin admin-dashboard";

            if (Session["UserId"] != null)
            {
                return Redirect("~/Menugroups");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}