using System.Linq;
using WinFormsApp.Models;

namespace WinFormsApp.Controllers
{
    public class ProveedorController
    {
        private AppDbContext _context;

        public ProveedorController()
        {
            _context = new AppDbContext();
        }

        public void AgregarProveedor(Proveedor proveedor)
        {
            _context.Proveedores.Add(proveedor);
            _context.SaveChanges();
        }

        public void EditarProveedor(Proveedor proveedor)
        {
            var proveedorExistente = _context.Proveedores.FirstOrDefault(p => p.Codigo == proveedor.Codigo);
            if (proveedorExistente != null)
            {
                proveedorExistente.Nombre = proveedor.Nombre;
                proveedorExistente.Direccion = proveedor.Direccion;
                proveedorExistente.Contacto = proveedor.Contacto;
                _context.SaveChanges();
            }
        }

        public void EliminarProveedor(int codigo)
        {
            var proveedor = _context.Proveedores.FirstOrDefault(p => p.Codigo == codigo);
            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);
                _context.SaveChanges();
            }
        }

        public IQueryable<Proveedor> ObtenerProveedores()
        {
            return _context.Proveedores.Include("Productos");
        }

        public Proveedor ObtenerProveedor(int codigo)
        {
            return _context.Proveedores.FirstOrDefault(p => p.Codigo == codigo);
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
