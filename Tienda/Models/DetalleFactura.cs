using WinFormsApp.Models;

public class DetalleFactura
{
    public int Id { get; set; }  // Asegúrate de tener una propiedad Id como clave primaria de detalle
    public int FacturaId { get; set; }  // Clave foránea hacia la factura, de tipo int
    public int ProductoId { get; set; }  // Clave foránea hacia Producto

    public Producto Producto { get; set; } // Relación con Producto
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }

    // Propiedad calculada para el subtotal
    public decimal Subtotal
    {
        get
        {
            return Cantidad * PrecioUnitario;
        }
    }

    // Relación: pertenece a una factura
    public Factura Factura { get; set; }
}
