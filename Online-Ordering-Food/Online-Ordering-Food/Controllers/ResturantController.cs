using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Online_Ordering_Food.Models;

namespace Online_Ordering_Food.Controllers
{
    public class ResturantController : Controller
    {
        //Home Page:
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult OOFHome()
        {
            return View();
        }

        //Regestration Page
        [HttpGet]
        [ActionName("Regestration")]
        public ActionResult Regestration_Get()
        {
            return View();
        }


        [HttpPost]
        [ActionName("Regestration")]
        public ActionResult Regestration_Post(UserInfo user)
        {
            using (ResturantEntities db = new ResturantEntities())
            {
                if (db.UserInfoes.Any(x => x.username == user.username) || db.UserInfoes.Any(y => y.EmailAddress == user.EmailAddress))
                {
                    ViewBag.message = "UserName OR Email Already Exist";
                    return View(user);
                }
                else
                {
                    db.UserInfoes.Add(user);
                    db.SaveChanges();
                }

            }
            return RedirectToAction("LogIn");
        }

        //login page
        [HttpGet]
        [ActionName("LogIn")]
        public ActionResult LogIn_Get()
        {

            return View();
        }

        [HttpPost]
        [ActionName("LogIn")]
        public ActionResult LogIn_Post(UserInfo user)
        {
            ResturantEntities db = new ResturantEntities();

            if ((user.username == "admin") && (user.Password == "admin1234"))
            {
                return RedirectToAction("AdminDashboard");
            }
            else if (db.UserInfoes.Any(x => x.username == user.username) && db.UserInfoes.Any(y => y.Password == user.Password))
            {
                return RedirectToAction("OOFHome");
            }
            else
            {
                ViewBag.message = "username or Password is INCORRECT";
                return View(user);
            }
        }


        // admin pages
        public ActionResult AdminDashboard()
        {
            return View();
        }
    }
}
