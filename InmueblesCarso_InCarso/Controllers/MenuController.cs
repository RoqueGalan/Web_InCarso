using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InmueblesCarso_InCarso.Models;

namespace InmueblesCarso_InCarso.Controllers
{
    public class MenuController : Controller
    {
        private InCarsoContext db = new InCarsoContext();

        // GET: Menu
        //public ActionResult Index()
        //{
        //    return PartialView("_MenuSuperior");
        //}
        [Authorize]
        public ActionResult Administrar()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult MenuRoot()
        {
            ViewBag.idioma = Session["idioma"];

            var lista = db.Carpetas.Where(a => a.CarpetaPadreID == null && a.Status).OrderBy(a => a.Orden);

            foreach (var item in lista.ToList())
            {

                item.Carpetas = item.Carpetas.Where(a => a.Status).OrderBy(a => a.Orden).ToList();
                item.Archivos = item.Archivos.Where(a => a.Status).OrderBy(a => a.Orden).ToList();
            }
            return PartialView("_MenuRoot", lista.ToList());
        }

        [AllowAnonymous]
        public ActionResult MenuHijo(Carpeta carpeta)
        {
            ViewBag.idioma = Session["idioma"];

            carpeta.Carpetas = carpeta.Carpetas.Where(a => a.Status).OrderBy(a => a.Orden).ToList();
            carpeta.Archivos = carpeta.Archivos.Where(a => a.Status).OrderBy(a => a.Orden).ToList();

            return PartialView("_MenuHijo", carpeta);
        }
    }
}