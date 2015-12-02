using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InmueblesCarso_InCarso.Models
{
    public class Carpeta
    {
        [Key]
        public int CarpetaID { get; set; }

        public string Clave { get; set; }
        public int Orden { get; set; }

        public string NombreES { get; set; }
        public string NombreEU { get; set; }


        public string DescripcionES { get; set; }
        public string DescripcionEU { get; set; }

        [ForeignKey("CarpetaPadre")]
        public int? CarpetaPadreID { get; set; }

        public bool EsItemDelMenu { get; set; }
        public bool Status { get; set; }


        /* relaciones */
        public virtual Carpeta CarpetaPadre { get; set; }
        public virtual ICollection<Carpeta> Carpetas { get; set; }
        public virtual ICollection<Archivo> Archivos { get; set; }
    }
}