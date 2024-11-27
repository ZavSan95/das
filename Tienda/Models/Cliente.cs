using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WinFormsApp.Models
{
    public class Cliente
    {
        [Key]
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Contacto { get; set; }

        // Métodos estáticos para operaciones relacionadas con el cliente
        public static void AgregarCliente(AppDbContext context, Cliente cliente)
        {
            context.Clientes.Add(cliente);
            context.SaveChanges();
        }

        public static void EditarCliente(AppDbContext context, Cliente cliente)
        {
            var clienteExistente = context.Clientes.FirstOrDefault(c => c.Codigo == cliente.Codigo);
            if (clienteExistente != null)
            {
                clienteExistente.Nombre = cliente.Nombre;
                clienteExistente.Direccion = cliente.Direccion;
                clienteExistente.Contacto = cliente.Contacto;
                context.SaveChanges();
            }
        }

        public static void EliminarCliente(AppDbContext context, int codigo)
        {
            var cliente = context.Clientes.FirstOrDefault(c => c.Codigo == codigo);
            if (cliente != null)
            {
                context.Clientes.Remove(cliente);
                context.SaveChanges();
            }
        }

        public static IQueryable<Cliente> ObtenerClientes(AppDbContext context)
        {
            return context.Clientes;
        }

        public static Cliente ObtenerCliente(AppDbContext context, int codigo)
        {
            return context.Clientes.FirstOrDefault(c => c.Codigo == codigo);
        }
    }
}

