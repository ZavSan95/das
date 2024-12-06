using MetroFramework.Forms;
using System;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp.Controllers;
using WinFormsApp.Models;

namespace Tienda.Views
{
    public partial class Proveedores : MetroForm
    {
        private ProveedorController _proveedorController;

        public Proveedores()
        {
            InitializeComponent();
            _proveedorController = new ProveedorController();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                var proveedorNuevo = new Proveedor
                {
                    Nombre = txtNombre.Text,
                    Direccion = txtDireccion.Text,
                    Contacto = txtContacto.Text
                };

                _proveedorController.AgregarProveedor(proveedorNuevo);
                CargarDatos(); // Recargar los datos del DataGridView después de agregar
                LimpiarCampos();
                MessageBox.Show("Proveedor creado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (ArgumentException ex) // Capturamos ArgumentException en lugar de InvalidOperationException
            {
                // Mostrar el mensaje de error en un MessageBox
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LimpiarCampos();    
            }
        }


        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dgvProveedores.SelectedRows.Count > 0)
                {
                    var proveedorSeleccionado = (Proveedor)dgvProveedores.SelectedRows[0].DataBoundItem;

                    proveedorSeleccionado.Nombre = txtNombre.Text;
                    proveedorSeleccionado.Direccion = txtDireccion.Text;
                    proveedorSeleccionado.Contacto = txtContacto.Text;

                    _proveedorController.EditarProveedor(proveedorSeleccionado);
                    CargarDatos(); // Recargar los datos después de editar
                    LimpiarCampos();
                    MessageBox.Show("Proveedor editado con éxito", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Selecciona un proveedor para editar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LimpiarCampos();
                }
            }
            catch (ArgumentException ex) // Capturar ArgumentException
            {
                // Mostrar el mensaje de error en un MessageBox
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LimpiarCampos();
            }
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dgvProveedores.SelectedRows.Count > 0)
                {
                    var proveedorSeleccionado = (Proveedor)dgvProveedores.SelectedRows[0].DataBoundItem;
                    _proveedorController.EliminarProveedor(proveedorSeleccionado.Codigo);
                    CargarDatos(); // Recargar los datos después de eliminar
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show("Selecciona un proveedor para eliminar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (ArgumentException ex) // Capturar ArgumentException
            {
                // Mostrar el mensaje de error en un MessageBox
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LimpiarCampos();
            }
        }



        #region AUXILIARES

        private void FormProveedor_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarDatos()
        {
            var proveedores = _proveedorController.ObtenerProveedores().ToList();
            dgvProveedores.DataSource = proveedores;

            // Ocultar la columna "Productos" en el DataGridView
            if (dgvProveedores.Columns.Contains("Productos"))
            {
                dgvProveedores.Columns["Productos"].Visible = false;
            }
        }

        private void dgvProveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var proveedorSeleccionado = dgvProveedores.Rows[e.RowIndex].DataBoundItem as Proveedor;
                if (proveedorSeleccionado != null)
                {
                    txtNombre.Text = proveedorSeleccionado.Nombre;
                    txtDireccion.Text = proveedorSeleccionado.Direccion;
                    txtContacto.Text = proveedorSeleccionado.Contacto;
                }
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtContacto.Text = string.Empty;
            txtDireccion.Text = string.Empty;
        }

        #endregion
    }
}
