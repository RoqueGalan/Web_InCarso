using InmueblesCarso_InCarso.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace InmueblesCarso_InCarso.Controllers
{
    public class UsuarioController : Controller
    {
        private InCarsoContext db = new InCarsoContext();



        /// <summary>
        /// Encripta una cadena (se utiliza para los passwords de los usuarios)
        /// </summary>
        /// <param name="cont">Cadena a encriptar</param>
        /// <returns>Regresa la cadena encriptada</returns>
        /// <remarks></remarks>
        public static string EncriptaPass(string cont)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = Encoding.UTF8.GetBytes(cont);
            bs = x.ComputeHash(bs);
            StringBuilder s = new StringBuilder();
            for (Int16 i = 0; i <= bs.Length - 1; i++)
            {
                s.Append(bs[i].ToString("x2").ToLower());
            }
            return s.ToString();
        }





        // GET: Usuario/IniciarSesion
        [AllowAnonymous]
        public ActionResult IniciarSesion()
        {
            return View("IniciarSesion");
        }

        // POST: Usuario/IniciarSesion
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult IniciarSesion([Bind(Include = "UserName,Password")] Usuario usuario)
        {
            //validar que usuario y contraseña existan


            usuario.Password = EncriptaPass(usuario.Password);



            if (db.Usuarios.Where(a => a.UserName == usuario.UserName && a.Password == usuario.Password).Count() == 1)
            {
                //el usuario es correcto
                var usuarioX = db.Usuarios.Where(u => u.UserName == usuario.UserName).First();



                FormsAuthentication.SetAuthCookie(usuarioX.UserName,false);

                return RedirectToAction("Administrar", "Menu");

            }

            //activar el error para 
            if (!String.IsNullOrEmpty(usuario.UserName) || !String.IsNullOrWhiteSpace(usuario.UserName))
            {
                ModelState.Remove("UserName");

            }

            ModelState.Remove("Password");
            ModelState.AddModelError("Password", "La contraseña es incorrecta");
            usuario.Password = "";

            return View("IniciarSesion", usuario);
        }

        [Authorize]
        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home", null);
        }

    }
}