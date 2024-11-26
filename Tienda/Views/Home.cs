using MetroFramework.Forms;
using System;
using System.Windows.Forms;
using WinFormsApp.Models;

namespace Tienda.Views
{
    public partial class Home : MetroForm
    {
        public Home()
        {
            InitializeComponent();
        }

        private void proveedoresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Proveedores formProveedor = new Proveedores();
            formProveedor.ShowDialog(); 
        }

        private void productosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormProductos formProductos = new FormProductos();
            formProductos.ShowDialog();
        }

        private void categoriasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormClientes formClientes = new FormClientes();
            formClientes.ShowDialog();
        }

        private void nuevaVentaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormVenta formVenta = new FormVenta();
            formVenta.ShowDialog();
        }

        private void informeVentasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormFacturas formInforme = new FormFacturas();
            formInforme.ShowDialog();
        }
    }
}
