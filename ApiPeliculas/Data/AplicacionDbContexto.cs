using ApiPeliculas.Modelos;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
    public class AplicacionDbContexto : DbContext
    {
        public AplicacionDbContexto(DbContextOptions<AplicacionDbContexto> options) : base(options)
        {

        }

        // Agregar los modelos aquí
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Pelicula> Pelicula { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
