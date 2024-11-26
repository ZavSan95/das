namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeyClientInFactura : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Facturas", "Cliente_Codigo", "dbo.Clientes");
            DropIndex("dbo.Facturas", new[] { "Cliente_Codigo" });
            RenameColumn(table: "dbo.Facturas", name: "Cliente_Codigo", newName: "ClienteCodigo");
            AlterColumn("dbo.Facturas", "ClienteCodigo", c => c.Int(nullable: false));
            CreateIndex("dbo.Facturas", "ClienteCodigo");
            AddForeignKey("dbo.Facturas", "ClienteCodigo", "dbo.Clientes", "Codigo", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Facturas", "ClienteCodigo", "dbo.Clientes");
            DropIndex("dbo.Facturas", new[] { "ClienteCodigo" });
            AlterColumn("dbo.Facturas", "ClienteCodigo", c => c.Int());
            RenameColumn(table: "dbo.Facturas", name: "ClienteCodigo", newName: "Cliente_Codigo");
            CreateIndex("dbo.Facturas", "Cliente_Codigo");
            AddForeignKey("dbo.Facturas", "Cliente_Codigo", "dbo.Clientes", "Codigo");
        }
    }
}
