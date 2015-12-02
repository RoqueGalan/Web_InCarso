using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace InmueblesCarso_InCarso.Models
{
    public class Usuario
    {
        [Key]
        public Int64 UsuarioID { get; set; }

        [Required]
        //[Remote("EsUsuarioDisponible", "Validacion")]
        [Display(Name = "Nombre de Usuario")]
        //[Remote("validarRegistroUnico", "Usuario", HttpMethod = "POST", ErrorMessage = "Ya existe un registro con este nombre. Intenta con otro.")]
        public string UserName { get; set; }

        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*_])[0-9a-zA-Z!@#$%^&*_0-9]{8,128}$", ErrorMessage = "Contraseña debe contener, Mayuscula, Número, Caracter Especial !@#$%^&*_ y 8 Caracteres Mínimo.")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        public bool Status { get; set; }

    }
}