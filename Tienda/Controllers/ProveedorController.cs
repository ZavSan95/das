using System;
using System.Linq;
using System.Windows.Forms;
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

            // Validar el formato del correo electrónico del contacto
            if (!Validaciones.EsCorreoValido(proveedor.Contacto))
            {
                throw new ArgumentException("El formato del correo electrónico no es válido.");
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
            // Iniciar una transacción
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Validar que el proveedor no sea nulo
                    if (proveedor == null)
                    {
                        throw new ArgumentException("Proveedor no válido.");
                    }

                    // Buscar el proveedor existente en la base de datos
                    var proveedorExistente = _context.Proveedores.FirstOrDefault(p => p.Codigo == proveedor.Codigo);
                    if (proveedorExistente == null)
                    {
                        throw new ArgumentException("El proveedor no existe.");
                    }

                    // Validar que los campos no estén vacíos
                    if (string.IsNullOrEmpty(proveedor.Nombre) ||
                        string.IsNullOrEmpty(proveedor.Direccion) ||
                        string.IsNullOrEmpty(proveedor.Contacto))
                    {
                        throw new ArgumentException("Todos los campos deben ser llenados.");
                    }

                    // Validar el formato del correo electrónico del contacto
                    if (!Validaciones.EsCorreoValido(proveedor.Contacto))
                    {
                        throw new ArgumentException("El formato del correo electrónico no es válido.");
                    }

                    // Si todas las validaciones son exitosas, actualiza los datos
                    proveedorExistente.Nombre = proveedor.Nombre;
                    proveedorExistente.Direccion = proveedor.Direccion;
                    proveedorExistente.Contacto = proveedor.Contacto;

                    // Guardar los cambios en la base de datos
                    _context.SaveChanges();

                    // Confirmar la transacción
                    transaction.Commit();
                }
                catch
                {
                    // Revertir la transacción si ocurre un error
                    transaction.Rollback();
                    throw; // Relanzar la excepción para manejarla en niveles superiores
                }
            }
        }




        public void EliminarProveedor(int codigo)
        {
            // Buscar el proveedor en la base de datos
            var proveedor = _context.Proveedores.FirstOrDefault(p => p.Codigo == codigo);
            if (proveedor == null)
            {
                throw new ArgumentException("El proveedor no existe.");
            }

            // Mostrar mensaje de confirmación
            var resultado = MessageBox.Show(
                $"¿Estás seguro de que deseas eliminar al proveedor '{proveedor.Nombre}'?",
                "Confirmar Eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            // Verificar la respuesta del usuario
            if (resultado == DialogResult.Yes)
            {
                // Eliminar el proveedor
                _context.Proveedores.Remove(proveedor);
                _context.SaveChanges();
                MessageBox.Show("Proveedor eliminado correctamente.", "Eliminación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Operación cancelada.", "Eliminación Cancelada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
