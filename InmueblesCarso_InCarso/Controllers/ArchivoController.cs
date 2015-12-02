using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using InmueblesCarso_InCarso.Models;
using System.IO;

namespace InmueblesCarso_InCarso.Controllers
{
    public class ArchivoController : Controller
    {
        private InCarsoContext db = new InCarsoContext();

        // GET: Archivo/Details/5
        [AllowAnonymous]

        public ActionResult Detalles(int? id, string idioma = "ES", bool modal = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archivo archivo = db.Archivos.Find(id);
            if (archivo == null)
            {
                return HttpNotFound();
            }


            if (modal)
            {
                ViewBag.idioma = idioma;
                return PartialView("_VisualizarModal", archivo);
            }
            else
            {
                ViewBag.idioma = Session["idioma"];
                return PartialView("_Visualizar", archivo);
            }

        }


        [AllowAnonymous]
        public FileStreamResult GetPDF(string nombrePDF)
        {

            nombrePDF = string.IsNullOrWhiteSpace(nombrePDF) ? "defaultPDF.pdf" : nombrePDF;
            var a = new Archivo();
            var rutaLocal = a.RutaGuardar;
            var rutaServ = Server.MapPath("~" + rutaLocal);

            string rutaReal = Path.Combine(rutaServ, nombrePDF);


            if (!System.IO.File.Exists(rutaReal))
            {
                rutaReal = Path.Combine(rutaServ, "defaultPDF.pdf");
            }

            FileStream fs = new FileStream(rutaReal, FileMode.Open, FileAccess.Read);


            return File(fs, "application/pdf");
        }


        // GET: Archivo/Create
        [Authorize]
        public ActionResult Crear(int id = 0)
        {
            var db = new InCarsoContext("AltConnection");

            if (id == 0) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Carpeta carpeta = db.Carpetas.Find(id);

            if (carpeta == null) return HttpNotFound();


            Archivo archivo = new Archivo()
            {
                Carpeta = carpeta,
                CarpetaID = carpeta.CarpetaID,
                FechaPublicacion = DateTime.UtcNow.ToShortDateString(),
                Status = true,
                Orden = carpeta.Archivos.Count + 1
            };

            return PartialView("_Crear", archivo);
        }

        // POST: Archivo/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear([Bind(Include = "ArchivoID,Orden,NombreES,NombreEU,DescripcionES,DescripcionEU,CarpetaID,RutaArchivoES,RutaArchivoEU,FechaPublicacion,Status")] Archivo archivo, HttpPostedFileBase FilePDF_ES, HttpPostedFileBase FilePDF_EN, string check_ES = "", string check_EN = "")
        {
            var db = new InCarsoContext("AltConnection");

            if (check_ES == "" && check_EN == "")
            {
                ModelState.AddModelError("", "Selecciona por lo menos un idioma.");
            }

            //if (check_ES != "" && FilePDF_ES == null)
            //{
            //    ModelState.AddModelError("FilePDF_ES", "Falta Archivo.");
            //}

            //if (check_EN != "" && FilePDF_EN == null)
            //{
            //    ModelState.AddModelError("FilePDF_EN", "Falta Archivo.");
            //}

            if (ModelState.IsValid)
            {
                /*GENERAR EL NOMBRE DEL DOCUMENTO MEDIANTE LAS RUTAS*/
                string nombreGen = "";
                List<string> listaNombre = new List<string>();
                var car = db.Carpetas.Find(archivo.CarpetaID);
                var sig = car.CarpetaPadre;
                listaNombre.Add(car.Clave);
                while (sig != null)
                {
                    listaNombre.Add(sig.Clave);
                    sig = sig.CarpetaPadre;
                }
                foreach (var item in listaNombre.Reverse<string>())
                    nombreGen += item + ".";

                var nGuid = Guid.NewGuid().ToString();

                if (FilePDF_ES != null)
                {
                    var extensionES = Path.GetExtension(FilePDF_ES.FileName);
                    archivo.RutaArchivoES = nombreGen + "[" + nGuid + "]" + "_ES" + extensionES;
                    var RutaDeGuardarES = Server.MapPath("~/" + archivo.RutaGuardar + archivo.RutaArchivoES);
                    FilePDF_ES.SaveAs(RutaDeGuardarES);
                }

                if (FilePDF_EN != null)
                {
                    var extensionEN = Path.GetExtension(FilePDF_EN.FileName);
                    archivo.RutaArchivoEU = nombreGen + "[" + nGuid + "]" + "_EN" + extensionEN;
                    var RutaDeGuardarEN = Server.MapPath("~/" + archivo.RutaGuardar + archivo.RutaArchivoEU);
                    FilePDF_EN.SaveAs(RutaDeGuardarEN);
                }

                db.Archivos.Add(archivo);
                db.SaveChanges();


                string ruta = Url.Action("Detalles", "Carpeta", new { id = car.CarpetaID });

                return Json(new { success = true, ruta = ruta, carpetaPadreID = car.CarpetaPadreID });

            }

            return PartialView("_Crear", archivo);
        }

        // GET: Archivo/Edit/5
        [Authorize]
        public ActionResult Editar(int? id)
        {
            var db = new InCarsoContext("AltConnection");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Archivo archivo = db.Archivos.Find(id);
            if (archivo == null)
            {
                return HttpNotFound();
            }

            ViewBag.checkES = "";
            ViewBag.checkEN = "";


            return PartialView("_Editar", archivo);
        }

        // POST: Archivo/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Editar([Bind(Include = "ArchivoID,Orden,NombreES,NombreEU,DescripcionES,DescripcionEU,CarpetaID,RutaArchivoES,RutaArchivoEU,FechaPublicacion,Status")] Archivo archivo, HttpPostedFileBase FilePDF_ES, HttpPostedFileBase FilePDF_EN, string check_ES = "", string check_EN = "", string check_FileES = "", string check_FileEN = "")
        {
            var db = new InCarsoContext("AltConnection");
            //validaciones 
            // si check_ES != "" actualizar el español
            //      si check_FileES != "" actualizar el archivo
            //          eliminar el archivo anterior

            // si check_EN != "" actualizar el inglés
            //      si check_FileEN != "" actualizar el archivo
            //          eliminar el archivo anterior


            if (check_FileES != "" && FilePDF_ES == null)
                ModelState.AddModelError("FilePDF_ES", "Falta Archivo Español.");

            if (check_FileEN != "" && FilePDF_EN == null)
                ModelState.AddModelError("FilePDF_EN", "Falta Archivo Inglés.");

            var archivoViejo = db.Archivos.Find(archivo.ArchivoID);

            if (ModelState.IsValid)
            {
                //campos que se pueden actualizar desde el inicio
                /*
                 * Orden
                 * Status
                 */
                archivoViejo.Orden = archivo.Orden;
                archivoViejo.Status = archivo.Status;

                /*GENERAR EL NOMBRE DEL DOCUMENTO MEDIANTE LAS RUTAS*/
                var nGuid = Guid.NewGuid().ToString();
                string nombreGen = "";
                List<string> listaNombre = new List<string>();
                var car = db.Carpetas.Find(archivoViejo.CarpetaID);
                var sig = car.CarpetaPadre;
                listaNombre.Add(car.Clave);
                while (sig != null)
                {
                    listaNombre.Add(sig.Clave);
                    sig = sig.CarpetaPadre;
                }
                foreach (var item in listaNombre.Reverse<string>())
                    nombreGen += item + ".";

                //ESPAÑOL
                if (check_ES != "")
                {
                    //campos que se van actualizar desde el inicio de ESPAÑOL
                    /*
                     * NombreES
                     * DescripcionES
                     */
                    archivoViejo.NombreES = archivo.NombreES;
                    archivoViejo.DescripcionES = archivo.NombreES;

                    //ARCHIVO
                    if (check_FileES != "")
                    {
                        //campos que se van actualizar desde el inicio de ESPAÑOL
                        /*
                         * RutaArchivoES
                         */
                        if (FilePDF_ES != null)
                        {
                            var extensionES = Path.GetExtension(FilePDF_ES.FileName);
                            archivo.RutaArchivoES = nombreGen + "[" + nGuid + "]" + "_ES" + extensionES;
                            var RutaDeGuardarES = Server.MapPath("~/" + archivo.RutaGuardar + archivo.RutaArchivoES);
                            FilePDF_ES.SaveAs(RutaDeGuardarES);

                            //eliminar el archivo
                            FileInfo infoOriginal = new FileInfo(Server.MapPath("~/" + archivoViejo.RutaGuardar + archivoViejo.RutaArchivoES));
                            if (infoOriginal.Exists)
                                infoOriginal.Delete();

                            archivoViejo.RutaArchivoES = archivo.RutaArchivoES;
                        }
                    }
                }

                //INGLES
                if (check_EN != "")
                {
                    //campos que se van actualizar desde el inicio de ESPAÑOL
                    /*
                     * NombreEU
                     * DescripcionEU
                     */
                    archivoViejo.NombreEU = archivo.NombreEU;
                    archivoViejo.DescripcionEU = archivo.NombreEU;

                    //ARCHIVO
                    if (check_FileEN != "")
                    {
                        //campos que se van actualizar desde el inicio de ESPAÑOL
                        /*
                         * RutaArchivoEU
                         */
                        if (FilePDF_EN != null)
                        {
                            var extensionEN = Path.GetExtension(FilePDF_EN.FileName);
                            archivo.RutaArchivoEU = nombreGen + "[" + nGuid + "]" + "_EN" + extensionEN;
                            var RutaDeGuardarEN = Server.MapPath("~/" + archivo.RutaGuardar + archivo.RutaArchivoEU);
                            FilePDF_EN.SaveAs(RutaDeGuardarEN);

                            //eliminar el archivo
                            FileInfo infoOriginal = new FileInfo(Server.MapPath("~/" + archivoViejo.RutaGuardar + archivoViejo.RutaArchivoEU));
                            if (infoOriginal.Exists)
                                infoOriginal.Delete();

                            archivoViejo.RutaArchivoEU = archivo.RutaArchivoEU;
                        }

                    }
                }

                db.Entry(archivoViejo).State = EntityState.Modified;
                db.SaveChanges();

                string ruta = Url.Action("Detalles", "Carpeta", new { id = car.CarpetaID });

                return Json(new { success = true, ruta = ruta, carpetaPadreID = car.CarpetaPadreID });

            }

            ViewBag.checkES = check_ES;
            ViewBag.checkEN = check_EN;

            return PartialView("_Editar", archivo);
        }

        // GET: Archivo/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Archivo archivo = db.Archivos.Find(id);
        //    if (archivo == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(archivo);
        //}

        //// POST: Archivo/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Archivo archivo = db.Archivos.Find(id);
        //    db.Archivos.Remove(archivo);
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
