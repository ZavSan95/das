using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WinFormsApp.Models
{
    public class Producto
    {
        [Key]
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int? CategoriaCodigo { get; set; }
        public virtual Categoria Categoria { get; set; }
        public int? ProveedorCodigo { get; set; }
        public virtual Proveedor Proveedor { get; set; }

        // Métodos para operaciones CRUD
        public static void Agregar(Producto producto, AppDbContext context)
        {
            context.Productos.Add(producto);
            context.SaveChanges();
        }

        public static void Editar(int codigoProducto, Producto productoNuevo, AppDbContext context)
        {
            var productoExistente = context.Productos.FirstOrDefault(p => p.Codigo == codigoProducto);
            if (productoExistente != null)
            {
                productoExistente.Nombre = productoNuevo.Nombre;
                productoExistente.Descripcion = productoNuevo.Descripcion;
                productoExistente.Precio = productoNuevo.Precio;
                productoExistente.Stock = productoNuevo.Stock;
                productoExistente.CategoriaCodigo = productoNuevo.CategoriaCodigo;
                productoExistente.ProveedorCodigo = productoNuevo.ProveedorCodigo;

                context.SaveChanges();
            }
            else
            {
                throw new Exception("El producto no fue encontrado en la base de datos.");
            }
        }

        public static void Eliminar(int codigo, AppDbContext context)
        {
            var producto = context.Productos.FirstOrDefault(p => p.Codigo == codigo);
            if (producto != null)
            {
                context.Productos.Remove(producto);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("El producto no fue encontrado en la base de datos.");
            }
        }

        public static List<Producto> ObtenerTodos(AppDbContext context)
        {
            return context.Productos.ToList();
        }
    }
}
