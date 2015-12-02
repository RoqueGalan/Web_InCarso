namespace InmueblesCarso_InCarso.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Archivo",
                c => new
                    {
                        ArchivoID = c.Int(nullable: false, identity: true),
                        NombreES = c.String(),
                        NombreEU = c.String(),
                        DescripcionES = c.String(),
                        DescripcionEU = c.String(),
                        CarpetaID = c.Int(nullable: false),
                        RutaArchivoES = c.String(),
                        RutaArchivoEU = c.String(),
                        FechaPublicacion = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ArchivoID)
                .ForeignKey("dbo.Carpeta", t => t.CarpetaID, cascadeDelete: true)
                .Index(t => t.CarpetaID);
            
            CreateTable(
                "dbo.Carpeta",
                c => new
                    {
                        CarpetaID = c.Int(nullable: false, identity: true),
                        Clave = c.String(),
                        Orden = c.Int(nullable: false),
                        NombreES = c.String(),
                        NombreEU = c.String(),
                        DescripcionES = c.String(),
                        DescripcionEU = c.String(),
                        CarpetaPadreID = c.Int(),
                        EsItemDelMenu = c.Boolean(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CarpetaPadre_CarpetaID = c.Int(),
                    })
                .PrimaryKey(t => t.CarpetaID)
                .ForeignKey("dbo.Carpeta", t => t.CarpetaPadre_CarpetaID)
                .Index(t => t.CarpetaPadre_CarpetaID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Carpeta", "CarpetaPadre_CarpetaID", "dbo.Carpeta");
            DropForeignKey("dbo.Archivo", "CarpetaID", "dbo.Carpeta");
            DropIndex("dbo.Carpeta", new[] { "CarpetaPadre_CarpetaID" });
            DropIndex("dbo.Archivo", new[] { "CarpetaID" });
            DropTable("dbo.Carpeta");
            DropTable("dbo.Archivo");
        }
    }
}
