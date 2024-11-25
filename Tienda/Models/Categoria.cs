using System.ComponentModel.DataAnnotations;

namespace WinFormsApp.Models
{
    public class Categoria
    {
        [Key]  // Marca esta propiedad como la clave primaria
        public int Codigo { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}
