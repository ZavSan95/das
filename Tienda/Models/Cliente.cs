using System.ComponentModel.DataAnnotations;

namespace WinFormsApp.Models
{
    public class Cliente
    {
        [Key]
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Contacto { get; set; }

    }
}
