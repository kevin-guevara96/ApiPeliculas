using ApiPeliculas.Modelos.Dtos;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiPeliculas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepositorio _ctRepo;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepositorio ctRepo, IMapper mapper)
        {
            _ctRepo = ctRepo;
            _mapper = mapper;
        }
         
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategorias()
        {
            var listaCategorias = _ctRepo.GetCategorias();

            var listaCategoriasDto = new List<CategoriaDto>();

            foreach (var item in listaCategorias)
            {
                listaCategoriasDto.Add(_mapper.Map<CategoriaDto>(item));
            }

            return Ok(listaCategoriasDto);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCategoria(int id)
        {
            var itemCategoria = _ctRepo.GetCategoria(id);

            if (itemCategoria == null)
            {
                return NotFound();
            }

            var categoriaDto = _mapper.Map<CategoriaDto>(itemCategoria);

            return Ok(categoriaDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoriaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategoria([FromBody] CrearCategoriaDto crearcategoriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (crearcategoriaDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_ctRepo.ExisteCategoria(crearcategoriaDto.Nombre))
            {
                ModelState.AddModelError("Existe", "La categoria ya existe");
                return StatusCode(StatusCodes.Status404NotFound, ModelState);
            }

            var categoria = _mapper.Map<Categoria>(crearcategoriaDto);

            if (!_ctRepo.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"No se guardo el registro {categoria.Nombre}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }

            return CreatedAtRoute("GetCategoria", new { id = categoria.Id });
        }

    }
}
