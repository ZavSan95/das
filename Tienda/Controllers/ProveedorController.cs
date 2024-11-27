using System;
using System.Linq;
using WinFormsApp.Models;

namespace WinFormsApp.Controllers
{
    public class ProveedorController
    {
        private readonly AppDbContext _context;

        public ProveedorController()
        {
            _context = new AppDbContext();
        }

        public IQueryable<Proveedor> ObtenerProveedores()
        {
            return Proveedor.ObtenerProveedores(_context);
        }

        public Proveedor ObtenerProveedor(int codigo)
        {
            return Proveedor.ObtenerProveedor(_context, codigo);
        }

        public void AgregarProveedor(Proveedor proveedor)
        {
            // Validar que los campos no estén vacíos
            if (string.IsNullOrEmpty(proveedor.Nombre) || string.IsNullOrEmpty(proveedor.Direccion) || string.IsNullOrEmpty(proveedor.Contacto))
            {
                throw new ArgumentException("Todos los campos deben ser llenados.");
            }

            // Validar que el proveedor no exista ya
            var proveedorExistente = _context.Proveedores.FirstOrDefault(p => p.Nombre == proveedor.Nombre);
            if (proveedorExistente != null)
            {
                throw new ArgumentException("Ya existe un proveedor con este nombre.");
            }

            // Si las validaciones son exitosas, agregar el proveedor
            Proveedor.Agregar(_context, proveedor);
        }


        public void EditarProveedor(Proveedor proveedor)
        {
            if (proveedor == null)
            {
                throw new ArgumentException("Proveedor no válido.");
            }

            var proveedorExistente = _context.Proveedores.FirstOrDefault(p => p.Codigo == proveedor.Codigo);
            if (proveedorExistente == null)
            {
                throw new ArgumentException("El proveedor no existe.");
            }

            proveedorExistente.Nombre = proveedor.Nombre;
            proveedorExistente.Direccion = proveedor.Direccion;
            proveedorExistente.Contacto = proveedor.Contacto;

            _context.SaveChanges();
        }

        public void EliminarProveedor(int codigo)
        {
            var proveedor = _context.Proveedores.FirstOrDefault(p => p.Codigo == codigo);
            if (proveedor == null)
            {
                throw new ArgumentException("El proveedor no existe.");
            }

            _context.Proveedores.Remove(proveedor);
            _context.SaveChanges();
        }


        // Método para obtener todos los proveedores con código y nombre
        public IQueryable<object> ObtenerProveedoresConCodigoYNombres()
        {
            return _context.Proveedores.Select(p => new
            {
                p.Codigo,
                p.Nombre
            });
        }
    }
}
