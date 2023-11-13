using ApiPeliculas.Modelos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IPeliculasRepositorio
    {
        ICollection<Pelicula> GetPeliculas();
        Pelicula GetPelicula(int peliculaId);
        bool ExistePelicula(string nombre);
        bool ExistePelicula(int id);
        bool CrearPelicula(Pelicula pelicula);
        bool ActualizarPelicula(Pelicula pelicula);
        bool BorrarPelicula(Pelicula pelicula);

        //Métodos para buscar peliculas en categoría y buscar pelicula por nombre
        ICollection<Pelicula> GetPeliculasEnCategoria(int catId);

        ICollection<Pelicula> BuscarPelicula(string nombre);

        bool Guardar();
    }
}
