namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Productoes", "CategoriaCodigo", "dbo.Categorias");
            DropForeignKey("dbo.Productoes", "ProveedorCodigo", "dbo.Proveedors");
            AddForeignKey("dbo.Productoes", "CategoriaCodigo", "dbo.Categorias", "Codigo");
            AddForeignKey("dbo.Productoes", "ProveedorCodigo", "dbo.Proveedors", "Codigo");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Productoes", "ProveedorCodigo", "dbo.Proveedors");
            DropForeignKey("dbo.Productoes", "CategoriaCodigo", "dbo.Categorias");
            AddForeignKey("dbo.Productoes", "ProveedorCodigo", "dbo.Proveedors", "Codigo", cascadeDelete: true);
            AddForeignKey("dbo.Productoes", "CategoriaCodigo", "dbo.Categorias", "Codigo", cascadeDelete: true);
        }
    }
}
