using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InmueblesCarso_InCarso.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string id = "ES")
        {
            Session["idioma"] = Session["idioma"] == null? "ES" : Session["idioma"];


            Session["idioma"] = id == "ES" ? "ES" : "EN";

            ViewBag.idioma = Session["idioma"];
            //var lista = System.IO.Directory.GetFiles(Server.MapPath("~/Content"));
            //var x = new System.IO.FileInfo(lista[0]);

            return View();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

    }
}