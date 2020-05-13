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
            ModelState.Clear();
            ViewBag.successmessage = "Registration SuccessFull";
            return View("Regestration");
        }
    }
}
