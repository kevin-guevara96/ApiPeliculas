﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Modelos
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string RutaImagen { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public enum TipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
        public TipoClasificacion clasificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        [ForeignKey("CategoriaId")]
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}
