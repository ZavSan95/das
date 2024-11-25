using System;
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
            _context.Productos.Add(producto);
            _context.SaveChanges();
        }

        public void EditarProducto(int codigoProducto, Producto productoNuevo)
        {
            var productoExistente = _context.Productos.FirstOrDefault(p => p.Codigo == codigoProducto);
            if (productoExistente != null)
            {
                // Actualizamos los valores del producto existente con los nuevos valores
                productoExistente.Nombre = productoNuevo.Nombre;
                productoExistente.Descripcion = productoNuevo.Descripcion;
                productoExistente.Precio = productoNuevo.Precio;
                productoExistente.Stock = productoNuevo.Stock;

                // Si se permite la relación de claves foráneas nulas, se puede manejar de esta forma
                productoExistente.CategoriaCodigo = productoNuevo.CategoriaCodigo;
                productoExistente.ProveedorCodigo = productoNuevo.ProveedorCodigo;

                // Guardamos los cambios en la base de datos
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("El producto no fue encontrado en la base de datos.");
            }
        }


        public void EliminarProducto(int codigo)
        {
            var producto = _context.Productos.FirstOrDefault(p => p.Codigo == codigo);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                _context.SaveChanges();
            }
        }

        public IQueryable<Producto> ObtenerProductosPorProveedor(int proveedorCodigo)
        {
            return _context.Productos.Where(p => p.ProveedorCodigo == proveedorCodigo);
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
    }
}
