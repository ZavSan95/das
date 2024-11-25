using System.Data.Entity;

namespace WinFormsApp.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }


        public AppDbContext() : base("name=ProveedorProductoDbContext")
        {
        }

        // Este método se llama cuando se está creando el modelo (configuración de relaciones, etc.)
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar las relaciones
            modelBuilder.Entity<Producto>()
                .HasRequired(p => p.Categoria)
                .WithMany()
                .HasForeignKey(p => p.CategoriaCodigo)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Producto>()
                .HasRequired(p => p.Proveedor)
                .WithMany(p => p.Productos)
                .HasForeignKey(p => p.ProveedorCodigo)
                .WillCascadeOnDelete(false);
        }
    }
}
