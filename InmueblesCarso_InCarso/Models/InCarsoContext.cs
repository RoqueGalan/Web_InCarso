using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace InmueblesCarso_InCarso.Models
{
    public class InCarsoContext : DbContext
    {

        public InCarsoContext() : base("name=GenConnection") { }

        public InCarsoContext(string connectionString) : base("name=" + connectionString) { }
        public DbSet<Carpeta> Carpetas { get; set; }
        public DbSet<Archivo> Archivos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }



    }
}