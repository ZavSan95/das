using System.Data.Entity;

namespace WinFormsApp.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Factura> Facturas { get; set; }  // Cambio: Factura -> Facturas
        public DbSet<DetalleFactura> DetalleFacturas { get; set; }

        public AppDbContext() : base("name=ProveedorProductoDbContext")
        {
        }

        // Este método se llama cuando se está creando el modelo (configuración de relaciones, etc.)
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar las relaciones de Producto
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

            // Configuración de la relación entre Factura y DetalleFactura
            modelBuilder.Entity<Factura>()
                .HasKey(f => f.Numero);  // Definir la clave primaria de Factura (Numero)

            modelBuilder.Entity<Factura>()
                .HasMany(f => f.Detalles)  // Una factura tiene muchos detalles
                .WithRequired(d => d.Factura)  // Un detalle pertenece a una factura
                .HasForeignKey(d => d.FacturaId)  // Relación con Factura
                .WillCascadeOnDelete(true);  // Configurar la eliminación en cascada

            modelBuilder.Entity<DetalleFactura>()
                .HasKey(d => d.Id);  // Definir la clave primaria de DetalleFactura

            modelBuilder.Entity<DetalleFactura>()
                .HasRequired(d => d.Producto)  // Un detalle tiene un producto
                .WithMany()  // Un producto puede estar en muchos detalles
                .HasForeignKey(d => d.ProductoId)  // Relación con Producto
                .WillCascadeOnDelete(false);  // No eliminar producto cuando se elimine el detalle
        }
    }
}
