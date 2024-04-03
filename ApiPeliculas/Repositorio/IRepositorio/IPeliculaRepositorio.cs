﻿using ApiPeliculas.Modelos;

namespace ApiPeliculas.Repositorio.IRepositorio
{
    public interface IPeliculaRepositorio
    {
        ICollection<Pelicula> GetPeliculas();
        Pelicula GetPelicula(int id);
        bool ExistePelicula(string nombre);
        bool ExistePelicula(int id);
        bool CrearPelicula(Pelicula Pelicula);
        bool ActualizarPelicula(Pelicula Pelicula);
        bool BorrarPelicula(Pelicula Pelicula);

        // Metodos para buscar peliculas en categoría y buscar pelicula por nombre
        ICollection<Pelicula> GetPeliculasEnCategoria(int categoriaid);
        ICollection<Pelicula> BuscarPelicula(string nombre);

        bool Guardar();
    }
}
