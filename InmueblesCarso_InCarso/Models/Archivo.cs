using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InmueblesCarso_InCarso.Models
{
    public class Archivo
    {
        [Key]
        public int ArchivoID { get; set; }

        public int Orden { get; set; }
        public string NombreES { get; set; }
        public string NombreEU { get; set; }

        public string DescripcionES { get; set; }
        public string DescripcionEU { get; set; }

        [ForeignKey("Carpeta")]
        public int CarpetaID { get; set; }

        public string RutaArchivoES { get; set; }
        public string RutaArchivoEU { get; set; }

        public string FechaPublicacion { get; set; }

        public bool Status { get; set; }

        /* relaciones */
        public Carpeta Carpeta { get; set; }




        [NotMapped]
        public string RutaGuardar
        {
            get
            {
                return "/Content/docx/pdf/";

            }
        }
    }
}