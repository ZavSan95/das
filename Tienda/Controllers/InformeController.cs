using System.Linq;
using WinFormsApp.Models;

namespace WinFormsApp.Controllers
{
    public class InformeController
    {
        // Instancia única de la clase
        private static InformeController _instance;

        // Objeto para manejar concurrencia en entornos multihilo
        private static readonly object _lock = new object();

        private readonly AppDbContext _context;

        // Constructor privado para evitar instanciación directa
        private InformeController()
        {
            _context = new AppDbContext();
        }

        // Propiedad para obtener la única instancia de la clase
        public static InformeController Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new InformeController();
                    }
                    return _instance;
                }
            }
        }

        public IQueryable<Factura> ObtenerFacturas()
        {
            return _context.Facturas;
        }

        public Cliente ObtenerCliente(int codigo)
        {
            return _context.Clientes.FirstOrDefault(c => c.Codigo == codigo);
        }
    }
}
