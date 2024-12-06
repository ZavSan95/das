using MetroFramework.Forms;
using System;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp.Controllers;
using WinFormsApp.Models;
using MetroFramework.Controls;

namespace Tienda.Views
{
    public partial class FormClientes : MetroForm
    {
        public FormClientes()
        {
            InitializeComponent();
        }

        private void FormClientes_Load(object sender, EventArgs e)
        {
            AplicarEstilosAlMetroGrid();
            CargarDatos();

        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar campos vacíos
                if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtDireccion.Text) || string.IsNullOrWhiteSpace(txtContacto.Text))
                {
                    MessageBox.Show("Todos los campos son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var clienteNuevo = new Cliente
                {
                    Nombre = txtNombre.Text,
                    Direccion = txtDireccion.Text,
                    Contacto = txtContacto.Text
                };

                // Agregar el cliente utilizando la instancia Singleton
                ClienteController.Instance.AgregarCliente(clienteNuevo);
                CargarDatos(); // Recargar los datos después de agregar
                LimpiarCampos();

                MessageBox.Show("Cliente agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al agregar el cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count > 0)
            {
                var clienteSeleccionado = (Cliente)dgvClientes.SelectedRows[0].DataBoundItem;

                // Validar campos vacíos antes de editar
                if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtDireccion.Text) || string.IsNullOrWhiteSpace(txtContacto.Text))
                {
                    MessageBox.Show("Todos los campos son obligatorios.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                clienteSeleccionado.Nombre = txtNombre.Text;
                clienteSeleccionado.Direccion = txtDireccion.Text;
                clienteSeleccionado.Contacto = txtContacto.Text;

                try
                {
                    ClienteController.Instance.EditarCliente(clienteSeleccionado);
                    CargarDatos(); // Recargar los datos después de editar
                    LimpiarCampos();
                    MessageBox.Show("Cliente editado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al editar el cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Selecciona un cliente para editar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvClientes.SelectedRows.Count > 0)
            {
                var clienteSeleccionado = (Cliente)dgvClientes.SelectedRows[0].DataBoundItem;

                var confirmResult = MessageBox.Show("¿Está seguro de eliminar este cliente?",
                                                     "Confirmación",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    try
                    {
                        ClienteController.Instance.EliminarCliente(clienteSeleccionado.Codigo);
                        CargarDatos(); // Recargar los datos después de eliminar
                        LimpiarCampos();
                        MessageBox.Show("Cliente eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al eliminar el cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Selecciona un cliente para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        #region AUXILIARES

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Asegurarse de que no se haya hecho clic en una celda vacía
            if (e.RowIndex >= 0)
            {
                try
                {
                    // Obtener el cliente de la fila seleccionada
                    var clienteSeleccionado = dgvClientes.Rows[e.RowIndex].DataBoundItem as Cliente;

                    if (clienteSeleccionado != null)
                    {
                        // Rellenar los campos de texto con la información del cliente seleccionado
                        txtNombre.Text = clienteSeleccionado.Nombre;
                        txtDireccion.Text = clienteSeleccionado.Direccion;
                        txtContacto.Text = clienteSeleccionado.Contacto;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ocurrió un error al seleccionar el cliente: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Método para cargar los datos al DataGridView
        private void CargarDatos()
        {
            try
            {
                // Obtener la lista de clientes a través del Singleton
                var clientes = ClienteController.Instance.ObtenerClientes().ToList();

                // Verificar si la lista está vacía
                if (clientes.Count == 0)
                {
                    // Limpiar el DataGridView si no hay datos
                    dgvClientes.DataSource = null;

                    // Opcional: Mostrar un mensaje al usuario
                    MessageBox.Show("No se encontraron clientes para mostrar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Asignar la lista local al DataGridView si hay datos
                    dgvClientes.DataSource = clientes;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtContacto.Text = string.Empty;

        }

        private void AplicarEstilosAlMetroGrid()
        {
            // Cambiar el tema del MetroGrid (puede ser Light o Dark)
            dgvClientes.Theme = MetroFramework.MetroThemeStyle.Light; // Light o Dark

            // Cambiar el color principal del MetroGrid (puede ser Blue, Green, Red, etc.)
            dgvClientes.Style = MetroFramework.MetroColorStyle.Blue; // Blue, Green, Red, etc.

            // Usar colores personalizados del estilo
            dgvClientes.UseStyleColors = true;

            // Opcional: Configurar otras propiedades visuales como el borde, el tamaño de las celdas, etc.
            dgvClientes.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue; // Color de fondo al seleccionar una celda
            dgvClientes.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;  // Color del texto al seleccionar una celda
        }

        #endregion
    }
}

