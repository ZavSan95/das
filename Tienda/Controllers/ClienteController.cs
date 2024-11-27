using System;
using System.Linq;
using WinFormsApp.Models;

namespace WinFormsApp.Controllers
{
    public class ClienteController
    {
        private static ClienteController _instance;
        private static readonly object _lock = new object();
        private readonly AppDbContext _context;

        private ClienteController()
        {
            _context = new AppDbContext();
        }

        public static ClienteController Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ClienteController();
                    }
                    return _instance;
                }
            }
        }

        public void AgregarCliente(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nombre) || string.IsNullOrWhiteSpace(cliente.Direccion) || string.IsNullOrWhiteSpace(cliente.Contacto))
            {
                throw new ArgumentException("Todos los campos son obligatorios.");
            }

            Cliente.AgregarCliente(_context, cliente);
        }

        public void EditarCliente(Cliente cliente)
        {
            if (cliente.Codigo <= 0 || string.IsNullOrWhiteSpace(cliente.Nombre) || string.IsNullOrWhiteSpace(cliente.Direccion) || string.IsNullOrWhiteSpace(cliente.Contacto))
            {
                throw new ArgumentException("Datos inválidos para la edición del cliente.");
            }

            Cliente.EditarCliente(_context, cliente);
        }

        public void EliminarCliente(int codigo)
        {
            if (codigo <= 0)
            {
                throw new ArgumentException("Código de cliente inválido.");
            }

            Cliente.EliminarCliente(_context, codigo);
        }

        public IQueryable<Cliente> ObtenerClientes()
        {
            return Cliente.ObtenerClientes(_context);
        }

        public Cliente ObtenerCliente(int codigo)
        {
            if (codigo <= 0)
            {
                throw new ArgumentException("Código de cliente inválido.");
            }

            return Cliente.ObtenerCliente(_context, codigo);
        }
    }
}

