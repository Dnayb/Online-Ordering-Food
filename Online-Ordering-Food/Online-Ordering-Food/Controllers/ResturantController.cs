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
        public ActionResult Index()
        {
            return View();
        }
    }
}
