namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FacturayDetalles : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DetalleFacturas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FacturaId = c.Int(nullable: false),
                        ProductoId = c.Int(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        PrecioUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Facturas", t => t.FacturaId, cascadeDelete: true)
                .ForeignKey("dbo.Productoes", t => t.ProductoId)
                .Index(t => t.FacturaId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.Facturas",
                c => new
                    {
                        Numero = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Cliente_Codigo = c.Int(),
                    })
                .PrimaryKey(t => t.Numero)
                .ForeignKey("dbo.Clientes", t => t.Cliente_Codigo)
                .Index(t => t.Cliente_Codigo);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DetalleFacturas", "ProductoId", "dbo.Productoes");
            DropForeignKey("dbo.DetalleFacturas", "FacturaId", "dbo.Facturas");
            DropForeignKey("dbo.Facturas", "Cliente_Codigo", "dbo.Clientes");
            DropIndex("dbo.Facturas", new[] { "Cliente_Codigo" });
            DropIndex("dbo.DetalleFacturas", new[] { "ProductoId" });
            DropIndex("dbo.DetalleFacturas", new[] { "FacturaId" });
            DropTable("dbo.Facturas");
            DropTable("dbo.DetalleFacturas");
        }
    }
}
