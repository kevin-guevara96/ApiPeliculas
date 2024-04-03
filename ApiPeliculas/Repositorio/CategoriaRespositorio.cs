using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;

namespace ApiPeliculas.Repositorio
{
    public class CategoriaRespositorio : ICategoriaRepositorio
    {
        private readonly ApplicationDbContext _bd;

        public CategoriaRespositorio(ApplicationDbContext db)
        {
            _bd = db;
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
            _bd.Categorias.Update(categoria);

            return Guardar();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _bd.Categorias.Remove(categoria);

            return Guardar();
        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.Now;

            _bd.Categorias.Add(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(string nombre)
        {
            bool valor = _bd.Categorias.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }
        public bool ExisteCategoria(int id)
        {
            return _bd.Categorias.Any(c => c.Id == id);
        }

        public Categoria GetCategoria(int id)
        {
            return _bd.Categorias.FirstOrDefault(c => c.Id == id);
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _bd.Categorias.OrderBy(c => c.Nombre).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}
