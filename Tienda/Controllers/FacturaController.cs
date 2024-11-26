using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using Tienda.Migrations;
using WinFormsApp.Models;

namespace WinFormsApp.Controllers
{
    public class FacturaController
    {
        private static FacturaController _instance;
        private readonly AppDbContext _context;

        // Hacer el constructor privado para evitar instanciación fuera de la clase
        private FacturaController(AppDbContext context)
        {
            _context = context;
        }

        // Método estático para acceder a la instancia única
        public static FacturaController GetInstance(AppDbContext context)
        {
            if (_instance == null)
            {
                _instance = new FacturaController(context);
            }
            return _instance;
        }

        public void CrearFactura(Factura factura, DataGridView dgvDetalles)
        {
            try
            {
                decimal totalFactura = 0;

                // Verificar que el cliente seleccionado ya exista en la base de datos
                var clienteExistente = _context.Clientes.SingleOrDefault(c => c.Codigo == factura.Cliente.Codigo);

                if (clienteExistente != null)
                {
                    // Asegurarse de que la propiedad Cliente está correctamente asociada
                    factura.Cliente = clienteExistente; // Asignar la referencia del cliente existente
                }
                else
                {
                    // Si el cliente no existe, deberías mostrar un mensaje de error o manejarlo según el caso
                    MessageBox.Show("Cliente no encontrado.");
                    return;
                }

                foreach (DataGridViewRow row in dgvDetalles.Rows)
                {
                    // Saltar la fila si está vacía
                    if (row.Cells[0].Value == null) continue;

                    // Obtener el código del producto y la cantidad
                    int codigoProducto = Convert.ToInt32(row.Cells[0].Value);  // Código del producto
                    int cantidad = Convert.ToInt32(row.Cells[2].Value);

                    // Obtener precio unitario y subtotal desde las celdas (índices 2 y 3)
                    decimal precioUnitario = Convert.ToDecimal(row.Cells[3].Value);  // Precio unitario
                    decimal subtotal = Convert.ToDecimal(row.Cells[4].Value);        // Subtotal

                    // Imprimir en consola para depuración
                    Console.WriteLine($"Fila: Producto {codigoProducto}");
                    Console.WriteLine($"Código Producto: {codigoProducto}");
                    Console.WriteLine($"Cantidad: {cantidad}");
                    Console.WriteLine($"Precio Unitario: {precioUnitario}");
                    Console.WriteLine($"Subtotal: {subtotal}");

                    // Buscar el producto por su código
                    var producto = _context.Productos.SingleOrDefault(p => p.Codigo == codigoProducto);

                    if (producto == null)
                    {
                        MessageBox.Show($"No se encontró el producto con código {codigoProducto}.");
                        return;
                    }

                    // Verificar que haya suficiente stock
                    if (producto.Stock < cantidad)
                    {
                        MessageBox.Show($"No hay suficiente stock para el producto {producto.Nombre}.");
                        return;
                    }

                    // Crear el detalle de la factura
                    var detalle = new DetalleFactura
                    {
                        ProductoId = producto.Codigo,
                        Cantidad = cantidad,
                        PrecioUnitario = precioUnitario
                    };

                    factura.Detalles.Add(detalle);

                    // Actualizar el stock del producto
                    producto.Stock -= cantidad;
                    _context.Entry(producto).State = System.Data.Entity.EntityState.Modified;

                    // Sumar el subtotal al total de la factura
                    totalFactura += subtotal;
                }

                // Establecer el total de la factura
                factura.Total = totalFactura;

                // Mostrar el total de la factura en un cartel
                MessageBox.Show($"Total de la factura: {totalFactura:C}");

                // Agregar la factura a la base de datos
                _context.Facturas.Add(factura);  // Esta línea debería agregar solo la factura, sin duplicar al cliente
                _context.SaveChanges();

                MessageBox.Show("Factura generada correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar la factura: {ex.Message}");
            }
        }


        // Método para obtener una factura por su ID
        public Factura ObtenerFacturaPorId(int id)
        {
            return _context.Facturas.Include("Detalles").FirstOrDefault(f => f.Numero == id);
        }

        public class FacturaConClienteDTO
        {
            public int Numero { get; set; }
            public DateTime Fecha { get; set; }
            public string NombreCliente { get; set; }
            public decimal Total { get; set; }
        }

        public IQueryable<FacturaConClienteDTO> ObtenerFacturasConCliente()
        {
            return _context.Facturas
                .Select(f => new FacturaConClienteDTO
                {
                    Numero = f.Numero,
                    Fecha = f.Fecha,
                    NombreCliente = f.Cliente != null ? f.Cliente.Nombre : "Cliente no disponible",
                    Total = f.Total
                });
        }

        // Obtener los detalles de una factura por número de factura con el producto incluido
        public List<DetalleFactura> ObtenerDetallesFactura(int numeroFactura)
        {
            var factura = _context.Facturas
                .Where(f => f.Numero == numeroFactura)
                .Include(f => f.Detalles.Select(d => d.Producto)) // Incluir producto dentro de detalles
                .FirstOrDefault(); // Recuperar la factura con los detalles y productos

            return factura?.Detalles.ToList(); // Retornar la lista de detalles si existe
        }


        // Método para obtener facturas por cliente
        public List<Factura> ObtenerFacturasPorCliente(int clienteCodigo)
        {
            return _context.Facturas
                .Where(f => f.ClienteCodigo == clienteCodigo)
                .Include(f => f.Cliente) // Cargar Cliente
                .Include(f => f.Detalles.Select(d => d.Producto)) // Cargar Detalles y Productos
                .ToList();
        }


    }

}
