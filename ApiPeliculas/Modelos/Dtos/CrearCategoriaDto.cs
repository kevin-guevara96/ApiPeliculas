﻿using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Modelos.Dtos
{
    public class CrearCategoriaDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio!")]
        [MaxLength(60, ErrorMessage = "El número máximos de caracteres es de 100!")]
        public string Nombre { get; set; }
    }
}
