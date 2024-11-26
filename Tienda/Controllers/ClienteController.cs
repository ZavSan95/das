using System.Linq;
using WinFormsApp.Models;

namespace WinFormsApp.Controllers
{
    public class ClienteController
    {
        // Instancia única de la clase
        private static ClienteController _instance;

        // Objeto para manejar concurrencia en entornos multihilo
        private static readonly object _lock = new object();

        private readonly AppDbContext _context;

        // Constructor privado para evitar instanciación directa
        private ClienteController()
        {
            _context = new AppDbContext();
        }

        // Propiedad para obtener la única instancia de la clase
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

        // Métodos de la clase
        public void AgregarCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            _context.SaveChanges();
        }

        public void EditarCliente(Cliente cliente)
        {
            var clienteExistente = _context.Clientes.FirstOrDefault(c => c.Codigo == cliente.Codigo);
            if (clienteExistente != null)
            {
                clienteExistente.Nombre = cliente.Nombre;
                clienteExistente.Direccion = cliente.Direccion;
                clienteExistente.Contacto = cliente.Contacto;
                _context.SaveChanges();
            }
        }

        public void EliminarCliente(int codigo)
        {
            var cliente = _context.Clientes.FirstOrDefault(c => c.Codigo == codigo);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                _context.SaveChanges();
            }
        }

        public IQueryable<Cliente> ObtenerClientes()
        {
            return _context.Clientes;
        }

        public Cliente ObtenerCliente(int codigo)
        {
            return _context.Clientes.FirstOrDefault(c => c.Codigo == codigo);
        }
    }
}
