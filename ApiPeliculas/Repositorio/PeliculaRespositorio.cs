using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Repositorio
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly ApplicationDbContext _bd;

        public PeliculaRepositorio(ApplicationDbContext db)
        {
            _bd = db;
        }

        public bool ActualizarPelicula(Pelicula pelicula)
        {
            _bd.Peliculas.Update(pelicula);

            return Guardar();
        }

        public bool BorrarPelicula(Pelicula pelicula)
        {
            _bd.Peliculas.Remove(pelicula);

            return Guardar();
        }

        public ICollection<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _bd.Peliculas;

            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));
            }
            
            return query.ToList();
        }

        public bool CrearPelicula(Pelicula Pelicula)
        {
            Pelicula.FechaCreacion = DateTime.Now;

            _bd.Peliculas.Add(Pelicula);
            return Guardar();
        }

        public bool ExistePelicula(string nombre)
        {
            bool valor = _bd.Peliculas.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }
        public bool ExistePelicula(int id)
        {
            return _bd.Peliculas.Any(c => c.Id == id);
        }

        public Pelicula GetPelicula(int id)
        {
            return _bd.Peliculas.FirstOrDefault(c => c.Id == id);
        }

        public ICollection<Pelicula> GetPeliculas()
        {
            return _bd.Peliculas.OrderBy(c => c.Nombre).ToList();
        }

        public ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaid)
        {
            return _bd.Peliculas.Include(ca => ca.Categoria).Where(ca => ca.CategoriaId == categoriaid).ToList();
        }

        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0 ? true : false;
        }
    }
}
