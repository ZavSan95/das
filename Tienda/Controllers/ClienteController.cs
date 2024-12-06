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
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(cliente.Nombre) || string.IsNullOrWhiteSpace(cliente.Direccion) || string.IsNullOrWhiteSpace(cliente.Contacto))
            {
                throw new ArgumentException("Todos los campos son obligatorios.");
            }

            // Validar longitud mínima para el nombre y contacto
            if (cliente.Nombre.Length < 3)
            {
                throw new ArgumentException("El nombre debe tener al menos 3 caracteres.");
            }

            if (cliente.Contacto.Length < 5)
            {
                throw new ArgumentException("El contacto debe tener al menos 5 caracteres.");
            }

            // Validar formato del contacto (por ejemplo, si es un teléfono o email)
            if (!EsFormatoValidoContacto(cliente.Contacto))
            {
                throw new ArgumentException("El formato del contacto es inválido.");
            }

            // Llamada al método para agregar el cliente
            Cliente.AgregarCliente(_context, cliente);
        }

        public void EditarCliente(Cliente cliente)
        {
            // Validar datos
            if (cliente.Codigo <= 0 || string.IsNullOrWhiteSpace(cliente.Nombre) || string.IsNullOrWhiteSpace(cliente.Direccion) || string.IsNullOrWhiteSpace(cliente.Contacto))
            {
                throw new ArgumentException("Datos inválidos para la edición del cliente.");
            }

            // Validar longitud mínima para el nombre y contacto
            if (cliente.Nombre.Length < 3)
            {
                throw new ArgumentException("El nombre debe tener al menos 3 caracteres.");
            }

            if (cliente.Contacto.Length < 5)
            {
                throw new ArgumentException("El contacto debe tener al menos 5 caracteres.");
            }

            // Validar formato del contacto
            if (!EsFormatoValidoContacto(cliente.Contacto))
            {
                throw new ArgumentException("El formato del contacto es inválido.");
            }

            // Llamada al método para editar el cliente
            Cliente.EditarCliente(_context, cliente);
        }

        public void EliminarCliente(int codigo)
        {
            if (codigo <= 0)
            {
                throw new ArgumentException("Código de cliente inválido.");
            }

            // Llamada al método para eliminar el cliente
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

        // Método auxiliar para validar el formato del contacto (puede ser un teléfono, email, etc.)
        private bool EsFormatoValidoContacto(string contacto)
        {
            // Validar un formato de teléfono básico (puedes adaptar según sea necesario)
            var telefonoRegex = @"^\+?\d{1,4}?[-.\s]?\(?\d{1,4}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,4}$";

            // Validar un formato de correo electrónico básico (puedes adaptarlo más si es necesario)
            var emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Si el contacto es un teléfono o un correo electrónico válido
            return System.Text.RegularExpressions.Regex.IsMatch(contacto, telefonoRegex) ||
                   System.Text.RegularExpressions.Regex.IsMatch(contacto, emailRegex);
        }
    }

}

