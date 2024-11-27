using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp.Models;

namespace WinFormsApp.Controllers
{
    public class ProductoController
    {
        private AppDbContext _context;

        public ProductoController()
        {
            _context = new AppDbContext();
        }

        public void AgregarProducto(Producto producto)
        {
            Producto.Agregar(producto, _context);
        }

        public void EditarProducto(int codigoProducto, Producto productoNuevo)
        {
            Producto.Editar(codigoProducto, productoNuevo, _context);
        }

        public void EliminarProducto(int codigo)
        {
            Producto.Eliminar(codigo, _context);
        }

        public List<Producto> ObtenerProductos()
        {
            return Producto.ObtenerTodos(_context);
        }

        // Método para cargar los productos en el DataGridView
        public void CargarProductos(DataGridView dgvProductos)
        {
            // Obtener todos los productos desde la base de datos
            var productos = _context.Productos.ToList();

            // Limpiar el DataGridView antes de agregar nuevas filas
            dgvProductos.Rows.Clear();

            // Iterar sobre cada producto para agregar la información al DataGridView
            foreach (var producto in productos)
            {
                // Obtener los datos de la categoría usando el código de categoría
                var categoria = _context.Categorias
                    .FirstOrDefault(c => c.Codigo == producto.CategoriaCodigo);

                // Obtener los datos del proveedor usando el código de proveedor
                var proveedor = _context.Proveedores
                    .FirstOrDefault(p => p.Codigo == producto.ProveedorCodigo);

                // Agregar una fila al DataGridView con los valores
                dgvProductos.Rows.Add(
                    producto.Codigo,
                    producto.Nombre,
                    producto.Descripcion,
                    producto.Precio,
                    producto.Stock,
                    categoria != null ? categoria.Nombre : "Sin categoría",  // Mostrar el nombre de la categoría
                    proveedor != null ? proveedor.Nombre : "Sin proveedor"   // Mostrar el nombre del proveedor
                );
            }
        }

        public Producto ObtenerProductoPorCodigo(int codigo)
        {
            return _context.Productos.FirstOrDefault(p => p.Codigo == codigo);
        }
    }
}
