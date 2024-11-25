using System;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp.Controllers;
using WinFormsApp.Models;

namespace Tienda.Views
{
    public partial class Proveedores : Form
    {
        private ProveedorController _proveedorController;

        public Proveedores()
        {
            InitializeComponent();
            _proveedorController = new ProveedorController();
        }

        // Cargar los datos de los proveedores en el DataGridView
        private void FormProveedor_Load(object sender, EventArgs e)
        {
            CargarDatos();
        }

        // Método para cargar los datos al DataGridView
        private void CargarDatos()
        {
            using (var context = new AppDbContext())
            {
                // Obtener la lista de proveedores y convertirla a una lista en memoria
                var proveedores = _proveedorController.ObtenerProveedores().ToList();

                // Asignar la lista local al DataGridView
                dgvProveedores.DataSource = proveedores;
            }
        }

        // Manejo del botón Agregar proveedor
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            var proveedorNuevo = new Proveedor
            {
                Nombre = txtNombre.Text,
                Direccion = txtDireccion.Text,
                Contacto = txtContacto.Text
            };

            _proveedorController.AgregarProveedor(proveedorNuevo);
            CargarDatos(); // Recargar los datos del DataGridView después de agregar
        }

        private void dgvProveedores_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Asegurarse de que no se haya hecho clic en una celda vacía
            if (e.RowIndex >= 0)
            {
                // Obtener el proveedor de la fila seleccionada
                var proveedorSeleccionado = dgvProveedores.Rows[e.RowIndex].DataBoundItem as Proveedor;

                if (proveedorSeleccionado != null)
                {
                    // Rellenar los campos de texto con la información del proveedor seleccionado
                    txtNombre.Text = proveedorSeleccionado.Nombre;
                    txtDireccion.Text = proveedorSeleccionado.Direccion;
                    txtContacto.Text = proveedorSeleccionado.Contacto;
                }
            }
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            if (dgvProveedores.SelectedRows.Count > 0)
            {
                var proveedorSeleccionado = (Proveedor)dgvProveedores.SelectedRows[0].DataBoundItem;

                proveedorSeleccionado.Nombre = txtNombre.Text;
                proveedorSeleccionado.Direccion = txtDireccion.Text;
                proveedorSeleccionado.Contacto = txtContacto.Text;

                _proveedorController.EditarProveedor(proveedorSeleccionado);
                CargarDatos(); // Recargar los datos después de editar
            }
            else
            {
                MessageBox.Show("Selecciona un proveedor para editar.");
            }
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (dgvProveedores.SelectedRows.Count > 0)
            {
                var proveedorSeleccionado = (Proveedor)dgvProveedores.SelectedRows[0].DataBoundItem;
                _proveedorController.EliminarProveedor(proveedorSeleccionado.Codigo);
                CargarDatos(); // Recargar los datos después de eliminar
            }
            else
            {
                MessageBox.Show("Selecciona un proveedor para eliminar.");
            }
        }
    }
}
