namespace Tienda.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCategoriaForeignKeyToProducto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        Codigo = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Descripcion = c.String(),
                    })
                .PrimaryKey(t => t.Codigo);
            
            AddColumn("dbo.Productoes", "CategoriaCodigo", c => c.Int(nullable: false));
            CreateIndex("dbo.Productoes", "CategoriaCodigo");
            AddForeignKey("dbo.Productoes", "CategoriaCodigo", "dbo.Categorias", "Codigo", cascadeDelete: true);
            DropColumn("dbo.Productoes", "Categoria");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Productoes", "Categoria", c => c.String());
            DropForeignKey("dbo.Productoes", "CategoriaCodigo", "dbo.Categorias");
            DropIndex("dbo.Productoes", new[] { "CategoriaCodigo" });
            DropColumn("dbo.Productoes", "CategoriaCodigo");
            DropTable("dbo.Categorias");
        }
    }
}
