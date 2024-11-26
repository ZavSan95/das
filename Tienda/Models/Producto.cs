using System;
using System.ComponentModel.DataAnnotations;

namespace WinFormsApp.Models
{
    public class Producto
    {
        [Key]
        public int Codigo { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        // Clave foránea a la categoría
        public int? CategoriaCodigo { get; set; }

        // Relación con la tabla Categoria
        public virtual Categoria Categoria { get; set; }  // Propiedad de navegación

        // Clave foránea
        public int? ProveedorCodigo { get; set; }
        public virtual Proveedor Proveedor { get; set; }

        // Sobrescribir ToString() para mostrar el código del producto
        public override string ToString()
        {
            return Codigo.ToString(); // O cualquier otra propiedad que desees mostrar
        }
    }


    // Clase adicional para los datos del DataGridView
    public class ProductoViewModel
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string CategoriaNombre { get; set; } // Para mostrar el nombre de la categoría
        public int CategoriaCodigo { get; set; } // Código de la categoría
        public string ProveedorNombre { get; set; } // Para mostrar el nombre del proveedor
        public int ProveedorCodigo { get; set; } // Código del proveedor
    }

}
