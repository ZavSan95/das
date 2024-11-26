using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
using WinFormsApp.Models;

public class Factura
{
    public int Numero { get; set; }
    public DateTime Fecha { get; set; }

    // Propiedad de clave foránea para Cliente
    [ForeignKey("Cliente")]
    public int ClienteCodigo { get; set; }  // Esta es la columna Cliente_Codigo en la base de datos

    public Cliente Cliente { get; set; }
    public decimal Total { get; set; }

    // Relación: una factura contiene uno o más detalles
    public List<DetalleFactura> Detalles { get; set; }

    public Factura()
    {
        Detalles = new List<DetalleFactura>();
    }

    // Método para calcular el total de la factura
    public void CalcularTotal()
    {
        Total = 0;
        foreach (var detalle in Detalles)
        {
            Total += detalle.Subtotal;
        }
    }
}
