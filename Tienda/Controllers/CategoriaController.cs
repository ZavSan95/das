using System.Linq;
using WinFormsApp.Models;

namespace WinFormsApp.Controllers
{
    public class CategoriaController
    {
        private AppDbContext _context;

        public CategoriaController()
        {
            _context = new AppDbContext();
        }

        // Método para agregar una nueva categoría
        public void AgregarCategoria(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            _context.SaveChanges();
        }

        // Método para editar una categoría existente
        public void EditarCategoria(Categoria categoria)
        {
            var categoriaExistente = _context.Categorias.FirstOrDefault(c => c.Codigo == categoria.Codigo);
            if (categoriaExistente != null)
            {
                categoriaExistente.Nombre = categoria.Nombre;
                categoriaExistente.Descripcion = categoria.Descripcion;
                _context.SaveChanges();
            }
        }

        // Método para eliminar una categoría
        public void EliminarCategoria(int codigo)
        {
            var categoria = _context.Categorias.FirstOrDefault(c => c.Codigo == codigo);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                _context.SaveChanges();
            }
        }

        // Método para obtener todas las categorías
        public IQueryable<Categoria> ObtenerCategorias()
        {
            return _context.Categorias;
        }

        // Método para obtener una categoría por código
        public Categoria ObtenerCategoria(int codigo)
        {
            return _context.Categorias.FirstOrDefault(c => c.Codigo == codigo);
        }
    }
}
