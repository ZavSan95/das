using MetroFramework.Forms;
using System;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp.Controllers;
using WinFormsApp.Models;


namespace Tienda.Views
{
    public partial class FormProductos : MetroForm
    {
        private readonly ProductoController _productoController = new ProductoController();
        private readonly CategoriaController _categoriaController = new CategoriaController();
        private readonly ProveedorController _proveedorController = new ProveedorController();

        public FormProductos()
        {
            InitializeComponent();

            #region AUXILIARES
            // Definir las columnas si aún no se han definido
            if (dgvProductos.Columns.Count == 0)
            {
                dgvProductos.Columns.Add("Codigo", "Código");
                dgvProductos.Columns.Add("Nombre", "Nombre");
                dgvProductos.Columns.Add("Descripcion", "Descripción");
                dgvProductos.Columns.Add("Precio", "Precio");
                dgvProductos.Columns.Add("Stock", "Stock");
                dgvProductos.Columns.Add("Categoria", "Categoría");
                dgvProductos.Columns.Add("Proveedor", "Proveedor");
            }

            // Inicializamos el controlador de productos
            _productoController = new ProductoController();

            // Llamar al método para cargar los productos en el DataGridView
            _productoController.CargarProductos(dgvProductos);

            #endregion
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validación de los campos
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El nombre del producto es obligatorio.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    MessageBox.Show("La descripción del producto es obligatoria.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
                {
                    MessageBox.Show("El precio debe ser un valor numérico positivo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
                {
                    MessageBox.Show("El stock debe ser un valor numérico no negativo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbCategorias.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar una categoría.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbProveedores.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un proveedor.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Crear un nuevo producto a partir de los datos del formulario
                var nuevoProducto = new Producto
                {
                    Nombre = txtNombre.Text,
                    Descripcion = txtDescripcion.Text,
                    Precio = precio,
                    Stock = stock,
                    CategoriaCodigo = (int)cmbCategorias.SelectedValue,
                    ProveedorCodigo = (int)cmbProveedores.SelectedValue
                };

                _productoController.AgregarProducto(nuevoProducto);
                CargarProductos(); // Actualizar la tabla
                LimpiarCampos();
                MessageBox.Show("Producto agregado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al agregar el producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.CurrentRow == null) throw new Exception("Seleccione un producto para editar.");

                // Validación de los campos
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    MessageBox.Show("El nombre del producto es obligatorio.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                {
                    MessageBox.Show("La descripción del producto es obligatoria.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio <= 0)
                {
                    MessageBox.Show("El precio debe ser un valor numérico positivo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
                {
                    MessageBox.Show("El stock debe ser un valor numérico no negativo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbCategorias.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar una categoría.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbProveedores.SelectedValue == null)
                {
                    MessageBox.Show("Debe seleccionar un proveedor.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var codigoProducto = Convert.ToInt32(dgvProductos.CurrentRow.Cells["Codigo"].Value);

                var productoEditado = new Producto
                {
                    Nombre = txtNombre.Text,
                    Descripcion = txtDescripcion.Text,
                    Precio = precio,
                    Stock = stock,
                    CategoriaCodigo = (int)cmbCategorias.SelectedValue,
                    ProveedorCodigo = (int)cmbProveedores.SelectedValue
                };

                _productoController.EditarProducto(codigoProducto, productoEditado);
                CargarProductos(); // Actualizar la tabla
                LimpiarCampos();
                MessageBox.Show("Producto editado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al editar el producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.CurrentRow == null) throw new Exception("Seleccione un producto para eliminar.");

                var codigoProducto = Convert.ToInt32(dgvProductos.CurrentRow.Cells["Codigo"].Value);

                var confirmResult = MessageBox.Show("¿Está seguro de eliminar este producto?",
                                                     "Confirmación",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question);

                if (confirmResult == DialogResult.Yes)
                {
                    _productoController.EliminarProducto(codigoProducto);
                    CargarProductos(); // Actualizar la tabla
                    LimpiarCampos();
                    MessageBox.Show("Producto eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar el producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #region AUXILIARES

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                var codigoProducto = Convert.ToInt32(dgvProductos.Rows[e.RowIndex].Cells["Codigo"].Value);

                var producto = _productoController.ObtenerProductoPorCodigo(codigoProducto);
                if (producto == null) throw new Exception("Producto no encontrado.");

                // Rellenar los campos con la información del producto seleccionado
                txtNombre.Text = producto.Nombre;
                txtDescripcion.Text = producto.Descripcion;
                txtPrecio.Text = producto.Precio.ToString("0.00");
                txtStock.Text = producto.Stock.ToString();
                cmbCategorias.SelectedValue = producto.CategoriaCodigo;
                cmbProveedores.SelectedValue = producto.ProveedorCodigo;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos del producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtPrecio.Text  = string.Empty;
            txtStock.Text = string.Empty;

            cmbCategorias.SelectedItem = null;
            cmbProveedores.SelectedItem = null;
        }

        private void CargarProductos()
        {
            // Cargar los productos al DataGridView
            _productoController.CargarProductos(dgvProductos);
        }

        private void FormProductos_Load(object sender, EventArgs e)
        {
            // Obtener todas las categorías y cargar en el ComboBox
            var categorias = _categoriaController.ObtenerCategorias().ToList();
            cmbCategorias.DataSource = categorias;
            cmbCategorias.DisplayMember = "Nombre";  // Muestra el nombre en el ComboBox
            cmbCategorias.ValueMember = "Codigo";   // Usa el Código como valor

            // Obtener proveedores y cargar en el ComboBox
            var proveedores = _proveedorController.ObtenerProveedoresConCodigoYNombres().ToList();
            cmbProveedores.DataSource = proveedores;
            cmbProveedores.DisplayMember = "Nombre";   // Mostrar el nombre del proveedor en el ComboBox
            cmbProveedores.ValueMember = "Codigo";    // Usar el código del proveedor como valor en el ComboBox
        }

        #endregion

    }
}

