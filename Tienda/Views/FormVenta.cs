using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinFormsApp.Controllers;
using WinFormsApp.Models;

namespace Tienda.Views
{

    public partial class FormVenta : MetroForm
    {
        private ProductoController productoController = new ProductoController();
        
        public FormVenta()
        {
            InitializeComponent();
            CargarProductosEnComboBox();
        }

        private void FormVenta_Load(object sender, EventArgs e)
        {
            // Cargar los clientes en el ComboBox
            cmbClientes.DataSource = ClienteController.Instance.ObtenerClientes().ToList();
            cmbClientes.DisplayMember = "Nombre";
            cmbClientes.ValueMember = "Codigo";

            // Inicializar DataGridView solo si no están previamente cargadas las columnas
            if (dgvDetalles.Columns.Count == 0)
            {
                dgvDetalles.Columns.Add("Codigo", "Codigo");
                dgvDetalles.Columns.Add("Producto", "Producto");
                dgvDetalles.Columns.Add("Cantidad", "Cantidad");
                dgvDetalles.Columns.Add("PrecioUnitario", "Precio Unitario");
                dgvDetalles.Columns.Add("Subtotal", "Subtotal");
            }
        }

        private void CargarProductosEnComboBox()
        {
            // Obtener los productos disponibles
            var productos = productoController.ObtenerProductos();

            // Limpiar el ComboBox antes de cargar los productos
            cmbProductos.Items.Clear();

            // Cargar el ComboBox con los productos
            foreach (var producto in productos)
            {
                // Se puede mostrar el código del producto, nombre y stock en el ComboBox
                cmbProductos.Items.Add(new ProductoComboBoxItem
                {
                    Codigo = producto.Codigo,
                    Nombre = producto.Nombre,
                    Stock = producto.Stock
                });

                // Establecer la propiedad de visualización
                cmbProductos.DisplayMember = "Descripcion";
                cmbProductos.ValueMember = "Codigo";
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Obtener el ProductoComboBoxItem seleccionado
            var productoItem = (ProductoComboBoxItem)cmbProductos.SelectedItem;

            // Buscar el producto real utilizando el código
            var producto = productoController.ObtenerProductoPorCodigo(productoItem.Codigo);

            if (producto == null)
            {
                MessageBox.Show("El producto seleccionado no existe.");
                return;
            }

            int cantidad = Convert.ToInt32(txtCantidad.Text);

            // Validar que haya suficiente stock
            if (producto.Stock < cantidad)
            {
                MessageBox.Show("No hay suficiente stock de este producto.");
                return;
            }

            // Calcular el subtotal
            var subtotal = producto.Precio * cantidad;

            // Agregar el detalle al DataGridView, guardando solo los datos necesarios
            dgvDetalles.Rows.Add(producto.Codigo, producto.Nombre, cantidad, producto.Precio, subtotal);

            // Actualizar el total de la factura
            ActualizarTotal();
            cmbProductos.SelectedIndex = -1;
            txtCantidad.Text = string.Empty;
        }



        private void ActualizarTotal()
        {
            decimal total = 0;
            foreach (DataGridViewRow row in dgvDetalles.Rows)
            {
                total += Convert.ToDecimal(row.Cells["Subtotal"].Value);
            }
            lblTotal.Text = $"Total: {total:C}";
        }

        private void btnFacturar_Click(object sender, EventArgs e)
        {
            // Crear la nueva factura
            var factura = new Factura
            {
                // Asignar la fecha actual
                Fecha = DateTime.Now,  
                Cliente = (Cliente)cmbClientes.SelectedItem
            };

            // Obtener la instancia única de FacturaController, pasando el contexto de la base de datos
            var facturaController = FacturaController.GetInstance(new AppDbContext());

            // Llamar al controlador para crear la factura y agregar los detalles
            facturaController.CrearFactura(factura, dgvDetalles);

            this.Close();
        }



    }

    // Clase auxiliar para mostrar el producto en el ComboBox
    public class ProductoComboBoxItem
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public int Stock { get; set; }

        // Propiedad de visualización para el ComboBox
        public string Descripcion => $"{Codigo} - {Nombre} - Stock: {Stock}";
    }
}
