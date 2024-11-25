namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Productoes",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Stock = c.Int(nullable: false),
                        Categoria = c.String(),
                        ProveedorCodigo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Codigo)
                .ForeignKey("dbo.Proveedors", t => t.ProveedorCodigo, cascadeDelete: true)
                .Index(t => t.ProveedorCodigo);
            
            CreateTable(
                "dbo.Proveedors",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Direccion = c.String(),
                        Contacto = c.String(),
                    })
                .PrimaryKey(t => t.Codigo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Productoes", "ProveedorCodigo", "dbo.Proveedors");
            DropIndex("dbo.Productoes", new[] { "ProveedorCodigo" });
            DropTable("dbo.Proveedors");
            DropTable("dbo.Productoes");
        }
    }
}
