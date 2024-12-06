using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp.Controllers;
using WinFormsApp.Models;

namespace Tienda.Views
{
    public partial class FormFacturas : MetroForm
    {
        // Instancia de FacturaController
        private readonly FacturaController _facturaController;

        public FormFacturas()
        {
            InitializeComponent();

            // Inicializa el controlador usando el patrón Singleton (pasamos el contexto solo una vez)
            _facturaController = FacturaController.GetInstance(new AppDbContext());

            // Cargar los clientes en el ComboBox
            cmbClientes.DataSource = ClienteController.Instance.ObtenerClientes().ToList();
            cmbClientes.DisplayMember = "Nombre";
            cmbClientes.ValueMember = "Codigo";
        }

        private void FormFacturas_Load(object sender, EventArgs e)
        {
            // Aplica el tema claro u oscuro al cargar
            AplicarEstilosAlMetroGrid();

            // Cargar las facturas en el DataGridView
            CargarFacturas();
        }

        private void dgvFacturas_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    // Validar que la celda seleccionada no sea nula
                    if (dgvFacturas.Rows[e.RowIndex].Cells["Numero"].Value != null)
                    {
                        // Obtener el número de factura seleccionado
                        int numeroFactura = (int)dgvFacturas.Rows[e.RowIndex].Cells["Numero"].Value;

                        // Llamar al controlador para obtener los detalles de la factura
                        var detalles = _facturaController.ObtenerDetallesFactura(numeroFactura);

                        // Mostrar los detalles de la factura
                        MostrarDetallesFactura(detalles);
                    }
                    else
                    {
                        MessageBox.Show("La fila seleccionada no contiene datos válidos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show("Ocurrió un error al procesar la selección. Verifique que la fila contiene datos válidos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Puedes registrar el error en un log si es necesario
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // También puedes registrar el error en un log
                Console.WriteLine(ex.Message);
            }
        }



        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // Obtener el código del cliente seleccionado en el ComboBox
            int codigoCliente = (int)cmbClientes.SelectedValue;

            // Llamar al controlador para obtener las facturas del cliente seleccionado
            var facturas = _facturaController.ObtenerFacturasPorCliente(codigoCliente);

            // Limpiar el DataGridView antes de cargar los resultados
            dgvFacturas.Rows.Clear();
            dgvDetallesFacturas.Rows.Clear();

            // Cargar las facturas en el DataGridView
            foreach (var factura in facturas)
            {
                // Acceder al nombre del cliente asociado a la factura
                string clienteNombre = factura.Cliente != null ? factura.Cliente.Nombre : "Cliente no disponible";

                // Agregar la factura al DataGridView
                dgvFacturas.Rows.Add(
                    factura.Numero,
                    factura.Fecha.ToShortDateString(),
                    clienteNombre,
                    factura.Total.ToString("C")
                );
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            // Limpiar el ComboBox y recargar todas las facturas
            cmbClientes.SelectedIndex = -1;
            CargarFacturas();
            dgvDetallesFacturas.Rows.Clear();
            dgvFacturas.Rows.Clear();
        }

        #region AUXILIARES
        private void CargarFacturas()
        {

            var facturas = _facturaController.ObtenerFacturasConCliente();


            // Limpiar el DataGridView antes de cargar nuevos datos
            dgvFacturas.Rows.Clear();

            // Definir las columnas del DataGridView si no se han definido previamente
            if (dgvFacturas.Columns.Count == 0)
            {
                dgvFacturas.Columns.Add("Numero", "Número");
                dgvFacturas.Columns.Add("Fecha", "Fecha");
                dgvFacturas.Columns.Add("Cliente", "Cliente");
                dgvFacturas.Columns.Add("Total", "Total");
            }


            // Iterar sobre las facturas y agregar cada una al DataGridView
            foreach (var factura in facturas)
            {
                // Accede al cliente asociado a la factura
                string clienteNombre = factura.NombreCliente != null ? factura.NombreCliente : "Cliente no disponible";

                // Crear una nueva fila en el DataGridView
                dgvFacturas.Rows.Add(
                    factura.Numero,
                    factura.Fecha.ToShortDateString(), // Muestra solo la fecha corta
                    clienteNombre,                     // Nombre del cliente
                    factura.Total.ToString("C")        // Formato de moneda para el total
                );
            }



        }

        private void MostrarDetallesFactura(List<DetalleFactura> detalles)
        {
            dgvDetallesFacturas.Rows.Clear();

            if (dgvDetallesFacturas.Columns.Count == 0)
            {
                dgvDetallesFacturas.Columns.Add("Producto", "Producto");
                dgvDetallesFacturas.Columns.Add("Cantidad", "Cantidad");
                dgvDetallesFacturas.Columns.Add("Subtotal", "Subtotal");
            }

            foreach (var detalle in detalles)
            {
                // Verificar que detalle.Producto no sea null antes de acceder a sus propiedades
                string productoNombre = detalle.Producto != null ? detalle.Producto.Nombre : "Producto no disponible";

                dgvDetallesFacturas.Rows.Add(
                    productoNombre,           // Nombre del producto
                    detalle.Cantidad,         // Cantidad
                    detalle.Subtotal.ToString("C") // Subtotal
                );
            }
        }
        private void AplicarEstilosAlMetroGrid()
        {
            // Cambiar el tema del MetroGrid (puede ser Light o Dark)
            dgvFacturas.Theme = MetroFramework.MetroThemeStyle.Light; // Light o Dark
            dgvDetallesFacturas.Theme = MetroFramework.MetroThemeStyle.Light; // Light o Dark

            // Cambiar el color principal del MetroGrid (puede ser Blue, Green, Red, etc.)
            dgvFacturas.Style = MetroFramework.MetroColorStyle.Blue; // Blue, Green, Red, etc.
            dgvDetallesFacturas.Style = MetroFramework.MetroColorStyle.Blue; // Blue, Green, Red, etc.

            // Usar colores personalizados del estilo
            dgvFacturas.UseStyleColors = true;
            dgvDetallesFacturas.UseStyleColors = true;

            // Opcional: Configurar otras propiedades visuales como el borde, el tamaño de las celdas, etc.
            dgvFacturas.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue; // Color de fondo al seleccionar una celda
            dgvFacturas.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;  // Color del texto al seleccionar una celda

            // Opcional: Configurar otras propiedades visuales como el borde, el tamaño de las celdas, etc.
            dgvDetallesFacturas.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue; // Color de fondo al seleccionar una celda
            dgvDetallesFacturas.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;  // Color del texto al seleccionar una celda
        }

        #endregion
    }
}
