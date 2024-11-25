using System;
using System.Linq;
using System.Windows.Forms;
using WinFormsApp.Controllers;
using WinFormsApp.Models;


namespace Tienda.Views
{
    public partial class FormProductos : Form
    {
        private CategoriaController categoriaController = new CategoriaController();
        private ProveedorController proveedorController = new ProveedorController();
        private ProductoController _productoController;
        public FormProductos()
        {
            InitializeComponent();

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
        }

        private void FormProductos_Load(object sender, EventArgs e)
        {
            // Obtener todas las categorías y cargar en el ComboBox
            var categorias = categoriaController.ObtenerCategorias().ToList();
            cmbCategorias.DataSource = categorias;
            cmbCategorias.DisplayMember = "Nombre";  // Muestra el nombre en el ComboBox
            cmbCategorias.ValueMember = "Codigo";   // Usa el Código como valor

            // Obtener proveedores y cargar en el ComboBox
            var proveedores = proveedorController.ObtenerProveedoresConCodigoYNombres().ToList();
            cmbProveedores.DataSource = proveedores;
            cmbProveedores.DisplayMember = "Nombre";   // Mostrar el nombre del proveedor en el ComboBox
            cmbProveedores.ValueMember = "Codigo";    // Usar el código del proveedor como valor en el ComboBox
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                // Crear un nuevo producto con los datos del formulario
                Producto nuevoProducto = new Producto
                {
                    Nombre = txtNombre.Text,
                    Descripcion = txtDescripcion.Text,
                    Precio = decimal.Parse(txtPrecio.Text),
                    Stock = int.Parse(txtStock.Text),
                    CategoriaCodigo = int.Parse(cmbCategorias.SelectedValue.ToString()),  // Selección de la categoría
                    ProveedorCodigo = int.Parse(cmbProveedores.SelectedValue.ToString())   // Selección del proveedor
                };

                // Llamar al controlador para agregar el producto
                ProductoController productoController = new ProductoController();
                productoController.AgregarProducto(nuevoProducto);

                // Llamar al método para cargar los productos en el DataGridView
                _productoController.CargarProductos(dgvProductos);

                // Mostrar mensaje de éxito
                MessageBox.Show("Producto agregado correctamente.");

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {

        }

        private void dgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que se hizo clic en una fila válida
            if (e.RowIndex >= 0)
            {
                // Obtener el índice de la fila seleccionada
                int rowIndex = e.RowIndex;

                // Obtener el código o identificador del producto seleccionado desde la celda de la columna "Código"
                int productoCodigo = Convert.ToInt32(dgvProductos.Rows[rowIndex].Cells["Codigo"].Value);

                // Usar Entity Framework para obtener el objeto del producto desde la base de datos
                using (var context = new AppDbContext())
                {
                    var productoSeleccionado = context.Productos
                        .FirstOrDefault(p => p.Codigo == productoCodigo);

                    if (productoSeleccionado != null)
                    {
                        // Rellenar los campos de texto con la información del producto seleccionado
                        txtNombre.Text = productoSeleccionado.Nombre;
                        txtDescripcion.Text = productoSeleccionado.Descripcion;
                        txtPrecio.Text = productoSeleccionado.Precio.ToString();
                        txtStock.Text = productoSeleccionado.Stock.ToString();

                        // Seleccionar el item correspondiente en el ComboBox de Categorías
                        cmbCategorias.SelectedValue = productoSeleccionado.CategoriaCodigo;

                        // Seleccionar el item correspondiente en el ComboBox de Proveedores
                        cmbProveedores.SelectedValue = productoSeleccionado.ProveedorCodigo;
                    }
                }
            }
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.CurrentRow != null)
                {
                    // Obtener el código del producto seleccionado
                    int codigoProducto = Convert.ToInt32(dgvProductos.CurrentRow.Cells["Codigo"].Value);

                    // Crear un nuevo objeto Producto con los valores editados
                    Producto productoEditado = new Producto
                    {
                        Nombre = txtNombre.Text,
                        Descripcion = txtDescripcion.Text,
                        Precio = decimal.Parse(txtPrecio.Text),
                        Stock = int.Parse(txtStock.Text),
                        CategoriaCodigo = (int)cmbCategorias.SelectedValue, // Se asegura de que no sea null
                        ProveedorCodigo = (int)cmbProveedores.SelectedValue // Se asegura de que no sea null
                    };

                    // Llamar al controlador para editar el producto en la base de datos
                    _productoController.EditarProducto(codigoProducto, productoEditado);

                    // Actualizar el DataGridView con los nuevos valores
                    _productoController.CargarProductos(dgvProductos);

                    // Informar al usuario que la edición fue exitosa
                    MessageBox.Show("Producto editado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Por favor, selecciona un producto de la lista.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
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
                if (dgvProductos.CurrentRow != null)
                {
                    // Obtener el código del producto seleccionado
                    int codigoProducto = Convert.ToInt32(dgvProductos.CurrentRow.Cells["Codigo"].Value);

                    // Confirmar que el usuario desea eliminar el producto
                    var result = MessageBox.Show("¿Está seguro de que desea eliminar este producto?",
                                                 "Confirmar eliminación",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Llamar al controlador para eliminar el producto
                        _productoController.EliminarProducto(codigoProducto);

                        // Actualizar el DataGridView después de la eliminación
                        _productoController.CargarProductos(dgvProductos);

                        // Informar al usuario que la eliminación fue exitosa
                        MessageBox.Show("Producto eliminado correctamente.",
                                        "Éxito",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, selecciona un producto de la lista para eliminarlo.",
                                    "Advertencia",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar el producto: {ex.Message}",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

    }
}
