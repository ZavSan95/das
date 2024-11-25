using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WinFormsApp.Models
{
    public class Proveedor
    {
        [Key]
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Contacto { get; set; }

        // Relación uno a muchos
        public virtual ICollection<Producto> Productos { get; set; }

        public Proveedor()
        {
            Productos = new HashSet<Producto>();
        }
    }
}
