using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Application.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index(string message)
        {
            ViewBag.err = message;
            return View();
        }
    }
}