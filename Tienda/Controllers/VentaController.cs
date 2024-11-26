using System;
using System.Collections.Generic;
using System.Linq;
using WinFormsApp.Models;

namespace WinFormsApp.Controllers
{
    public class VentaController
    {
        private readonly AppDbContext _context;

        public VentaController()
        {
            _context = new AppDbContext();
        }

        public void RegistrarVenta(Factura factura)
        {
            // Validar que no haya detalles sin producto o cantidad
            foreach (var detalle in factura.Detalles)
            {
                if (detalle.Cantidad <= 0 || detalle.Producto == null)
                {
                    throw new Exception("Cada detalle debe tener un producto válido y una cantidad mayor a 0.");
                }

                // Verificar si hay stock suficiente para cada producto
                var producto = _context.Productos.FirstOrDefault(p => p.Codigo == detalle.Producto.Codigo);
                if (producto == null || producto.Stock < detalle.Cantidad)
                {
                    throw new Exception($"Stock insuficiente para el producto: {detalle.Producto.Nombre}");
                }

                // Actualizar el stock del producto
                producto.Stock -= detalle.Cantidad;
            }

            // Calcular el total de la factura
            factura.CalcularTotal();

            // Registrar la factura en la base de datos
            //_context.Facturas.Add(factura);
            //_context.SaveChanges();
        }
    }
}
