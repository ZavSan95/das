using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WinFormsApp.Models
{
    public class Proveedor
    {
        [Key]
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Contacto { get; set; }

        // Relación uno a muchos
        public virtual ICollection<Producto> Productos { get; set; }

        public Proveedor()
        {
            Productos = new HashSet<Producto>();
        }

        // Métodos CRUD en el modelo
        public static void Agregar(AppDbContext context, Proveedor proveedor)
        {
            context.Proveedores.Add(proveedor);
            context.SaveChanges();
        }

        public static void Editar(AppDbContext context, Proveedor proveedor)
        {
            var proveedorExistente = context.Proveedores.FirstOrDefault(p => p.Codigo == proveedor.Codigo);
            if (proveedorExistente != null)
            {
                proveedorExistente.Nombre = proveedor.Nombre;
                proveedorExistente.Direccion = proveedor.Direccion;
                proveedorExistente.Contacto = proveedor.Contacto;
                context.SaveChanges();
            }
        }

        public static void Eliminar(AppDbContext context, int codigo)
        {
            var proveedor = context.Proveedores.FirstOrDefault(p => p.Codigo == codigo);
            if (proveedor != null)
            {
                context.Proveedores.Remove(proveedor);
                context.SaveChanges();
            }
        }

        public static IQueryable<Proveedor> ObtenerProveedores(AppDbContext context)
        {
            return context.Proveedores.Include("Productos");
        }

        public static Proveedor ObtenerProveedor(AppDbContext context, int codigo)
        {
            return context.Proveedores.FirstOrDefault(p => p.Codigo == codigo);
        }

    }
}
