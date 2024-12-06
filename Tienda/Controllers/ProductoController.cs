using System;
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
            // Validar los datos antes de agregar el producto
            if (string.IsNullOrEmpty(producto.Nombre))
            {
                throw new ArgumentException("El nombre del producto es obligatorio.");
            }

            if (producto.Precio <= 0)
            {
                throw new ArgumentException("El precio debe ser mayor a cero.");
            }

            if (producto.Stock < 0)
            {
                throw new ArgumentException("El stock no puede ser negativo.");
            }

            if (producto.CategoriaCodigo == null)
            {
                throw new ArgumentException("El producto debe tener una categoría.");
            }

            if (producto.ProveedorCodigo == null)
            {
                throw new ArgumentException("El producto debe tener un proveedor.");
            }

            // Si todas las validaciones son correctas, agregar el producto
            Producto.Agregar(producto, _context);
        }

        public void EditarProducto(int codigoProducto, Producto productoNuevo)
        {
            // Validar los datos antes de editar el producto
            if (string.IsNullOrEmpty(productoNuevo.Nombre))
            {
                throw new ArgumentException("El nombre del producto es obligatorio.");
            }

            if (productoNuevo.Precio <= 0)
            {
                throw new ArgumentException("El precio debe ser mayor a cero.");
            }

            if (productoNuevo.Stock < 0)
            {
                throw new ArgumentException("El stock no puede ser negativo.");
            }

            if (productoNuevo.CategoriaCodigo == null)
            {
                throw new ArgumentException("El producto debe tener una categoría.");
            }

            if (productoNuevo.ProveedorCodigo == null)
            {
                throw new ArgumentException("El producto debe tener un proveedor.");
            }

            // Si todas las validaciones son correctas, editar el producto
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
            var productos = _context.Productos.ToList();
            dgvProductos.Rows.Clear();

            foreach (var producto in productos)
            {
                var categoria = _context.Categorias.FirstOrDefault(c => c.Codigo == producto.CategoriaCodigo);
                var proveedor = _context.Proveedores.FirstOrDefault(p => p.Codigo == producto.ProveedorCodigo);

                dgvProductos.Rows.Add(
                    producto.Codigo,
                    producto.Nombre,
                    producto.Descripcion,
                    producto.Precio,
                    producto.Stock,
                    categoria != null ? categoria.Nombre : "Sin categoría",
                    proveedor != null ? proveedor.Nombre : "Sin proveedor"
                );
            }
        }

        public Producto ObtenerProductoPorCodigo(int codigo)
        {
            return _context.Productos.FirstOrDefault(p => p.Codigo == codigo);
        }
    }

}
