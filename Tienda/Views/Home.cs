using System;
using System.Windows.Forms;
using WinFormsApp.Models;

namespace Tienda.Views
{
    public partial class Home : Form
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
    }
}
