using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InmueblesCarso_InCarso.Models;

namespace InmueblesCarso_InCarso.Controllers
{
    public class CarpetaController : Controller
    {

        private InCarsoContext db = new InCarsoContext();

        // GET: Carpeta/Detalles/5
         [Authorize]
        public ActionResult Detalles(int id = 0)
        {
            var db = new InCarsoContext("AltConnection");

            Carpeta carpeta = null;
            PartialViewResult _vista = new PartialViewResult();

            if (id == 0)
                _vista = PartialView("_Lista", db.Carpetas.Where(a => a.CarpetaPadreID == null).ToList());
            else
            {
                carpeta = db.Carpetas.Find(id);

                if (carpeta == null)
                    _vista = PartialView("_Lista", db.Carpetas.Where(a => a.CarpetaPadreID == null).ToList());

                List<string> listabreadcrumb = new List<string>();

                var siguiente = carpeta.CarpetaPadre;

                while (siguiente != null)
                {
                    var link = "<a href='";
                    link += Url.Action("Detalles", "Carpeta", new { id = siguiente.CarpetaID });
                    link += "' class = 'renderCarpeta'>";
                    link += siguiente.NombreES;
                    link += "</a>";

                    listabreadcrumb.Add(link);
                    siguiente = siguiente.CarpetaPadre;
                }

                ViewBag.ListaLinks = listabreadcrumb;

                _vista = PartialView("_Detalles", carpeta);
            }

            return _vista;
        }



        // GET: Carpeta/Create
         [Authorize]
        public ActionResult Crear(int id = 0)
        {
            var db = new InCarsoContext("AltConnection");

            Carpeta carpeta = new Carpeta()
            {
                EsItemDelMenu = true,
                Status = true,
            };

            if (id == 0)
            {
                carpeta.CarpetaPadreID = null;
                carpeta.Orden = db.Carpetas.Where(a => a.CarpetaPadreID == null).Count() + 1;
            }
            else
            {
                carpeta.CarpetaPadreID = id;
                carpeta.Orden = db.Carpetas.Where(a => a.CarpetaPadreID == id).Count() + 1;
            }


            return PartialView("_Crear", carpeta);
        }


        // POST: Carpeta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Crear([Bind(Include = "CarpetaID,Clave,Orden,NombreES,NombreEU,DescripcionES,DescripcionEU,CarpetaPadreID,EsItemDelMenu,Status")] Carpeta carpeta)
        {
            var db = new InCarsoContext("AltConnection");

            if (ModelState.IsValid)
            {
                db.Carpetas.Add(carpeta);
                db.SaveChanges();

                string ruta = Url.Action("Detalles", "Carpeta", new { id = carpeta.CarpetaID });

                return Json(new { success = true, ruta = ruta, carpetaPadreID = carpeta.CarpetaPadreID });
            }

            return PartialView("_Crear", carpeta);
        }




        // GET: Carpeta/Edit/5
         [Authorize]
        public ActionResult Editar(int? id)
        {
            var db = new InCarsoContext("AltConnection");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Carpeta carpeta = db.Carpetas.Find(id);

            if (carpeta == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Editar", carpeta);
        }

        // POST: Carpeta/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Editar([Bind(Include = "CarpetaID,Clave,Orden,NombreES,NombreEU,DescripcionES,DescripcionEU,CarpetaPadreID,EsItemDelMenu,Status")] Carpeta carpeta)
        {
            var db = new InCarsoContext("AltConnection");

            if (ModelState.IsValid)
            {
                db.Entry(carpeta).State = EntityState.Modified;
                db.SaveChanges();

                string ruta = Url.Action("Detalles", "Carpeta", new { id = carpeta.CarpetaID });

                return Json(new { success = true, ruta = ruta, carpetaPadreID = carpeta.CarpetaPadreID });

            }
            return PartialView("_Editar", carpeta);
        }

        //// GET: Carpeta/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Carpeta carpeta = db.Carpetas.Find(id);
        //    if (carpeta == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(carpeta);
        //}

        //// POST: Carpeta/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Carpeta carpeta = db.Carpetas.Find(id);
        //    db.Carpetas.Remove(carpeta);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
