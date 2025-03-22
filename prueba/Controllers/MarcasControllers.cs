using Microsoft.AspNetCore.Mvc;
using prueba.Entities;
using prueba.Interfaces;
using prueba.Helpers;
using prueba.Extensions;
using prueba.Errors;
using prueba.Formatos;
using prueba.Dto;


namespace prueba.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    public class MarcasController : ApiControllerBase
    {
        private readonly IMarcaRespository _repo;
        private readonly IMarcaFormatterService _formatterService;
        private readonly ILogger<MarcasController> _logger;

        public MarcasController(
            IMarcaRespository repo,
            IMarcaFormatterService formatterService,
            ILogger<MarcasController> logger)
        {
            _repo = repo;
            _formatterService = formatterService;
            _logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetMarcas([FromQuery] PaginationParams paginationParams)
        {
            try
            {
                if (!ModelState.IsValid)
                    return HandleBadRequest("Invalid pagination parameters");

                var (marcas, metadata) = await _repo.ObtenerTodosAsync(paginationParams);

                if (!marcas.Any())
                    return HandleNotFound("No se encontraron marcas");

                 var marcasFormateadas = marcas.Select(m => _formatterService.FormatMarca(new MarcaDTO
                {
                    Id = m.Id,
                    Nombre = m.Nombre!,
                    Pais = m.Pais!,
                    AnioFundacion = m.AnioFundacion,
                    SedeCentral = m.SedeCentral!,
                    UrlLogo = m.UrlLogo!,
                    SitioWeb = m.SitioWeb!,
                    Descripcion = m.Descripcion!,
                    EsMarcaLujo = m.EsMarcaLujo,
                    FechaCreacion = m.FechaCreacion,
                    FechaActualizacion = m.FechaActualizacion ?? DateTime.UtcNow
                }));

                var response = new
                {
                    pagination = metadata,
                    data = marcasFormateadas
                };

                return HandleSuccess("Marcas obtenidas exitosamente", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting marcas with params {@PaginationParams}", paginationParams);
                throw;
            }
            }

            [HttpGet("{id}")]
            [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
            [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
            public async Task<ActionResult<ApiResponse>> GetMarcaById(int id)
            {
                try
                {
                    if (id <= 0)
                    {
                        return BadRequest(new ApiResponse(
                            mensaje: "El ID proporcionado no es válido",
                            exito: false
                        ));
                    }

                    var marca = await _repo.ObtenerPorIdAsync(id);

                    if (marca == null)
                    {
                        return NotFound(new ApiResponse(
                            mensaje: $"No se encontró la marca con ID {id}",
                            exito: false
                        ));
                    }

                    var marcaDto = new MarcaDTO
                    {
                        Id = marca.Id,
                        Nombre = marca.Nombre!,
                        Pais = marca.Pais!,
                        AnioFundacion = marca.AnioFundacion,
                        SedeCentral = marca.SedeCentral!,
                        UrlLogo = marca.UrlLogo!,
                        SitioWeb = marca.SitioWeb!,
                        Descripcion = marca.Descripcion!,
                        EsMarcaLujo = marca.EsMarcaLujo,
                        FechaCreacion = marca.FechaCreacion,
                        FechaActualizacion = marca.FechaActualizacion ?? DateTime.UtcNow
                    };



                    return Ok(new ApiResponse(
                        mensaje: "Marca obtenida exitosamente",
                        exito: true,
                        datos: _formatterService.FormatMarca(marcaDto)
                    ));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al obtener la marca con ID {Id}", id);
                    throw;
                }
            }

            [HttpPost]
            [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
            [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
            public async Task<ActionResult<ApiResponse>> CrearMarca([FromBody] MarcaDTO marcaDto)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(marcaDto.Nombre))
                    {
                        return BadRequest(new ApiResponse(
                            mensaje: "El nombre de la marca es requerido",
                            exito: false
                        ));
                    }

                    if (string.IsNullOrWhiteSpace(marcaDto.Pais))
                    {
                        return BadRequest(new ApiResponse(
                            mensaje: "El país es requerido",
                            exito: false
                        ));
                    }

                    if (string.IsNullOrWhiteSpace(marcaDto.SedeCentral))
                    {
                        return BadRequest(new ApiResponse(
                            mensaje: "La sede central es requerida",
                            exito: false
                        ));
                    }

                    if (marcaDto.AnioFundacion <= 1800 || marcaDto.AnioFundacion > DateTime.Now.Year)
                    {
                        return BadRequest(new ApiResponse(
                            mensaje: "El año de fundación debe estar entre 1800 y el año actual",
                            exito: false
                        ));
                    }

                    if (await _repo.ExistePorNombreAsync(marcaDto.Nombre))
                    {
                        return BadRequest(new ApiResponse(
                            mensaje: "Ya existe una marca con este nombre",
                            exito: false
                        ));
                    }

                    var marca = new Marca
                    {
                        Nombre = marcaDto.Nombre,
                        Pais = marcaDto.Pais,
                        AnioFundacion = marcaDto.AnioFundacion,
                        SedeCentral = marcaDto.SedeCentral,
                        UrlLogo = marcaDto.UrlLogo,
                        SitioWeb = marcaDto.SitioWeb,
                        Descripcion = marcaDto.Descripcion,
                        EsMarcaLujo = marcaDto.EsMarcaLujo,
                        FechaCreacion = DateTime.UtcNow
                    };

                    var nuevaMarca = await _repo.CrearAsync(marca);
                    var marcaDtoResponse = new MarcaDTO
                    {
                        Id = nuevaMarca.Id,
                        Nombre = nuevaMarca.Nombre!,
                        Pais = nuevaMarca.Pais!,
                        AnioFundacion = nuevaMarca.AnioFundacion,
                        SedeCentral = nuevaMarca.SedeCentral!,
                        UrlLogo = nuevaMarca.UrlLogo!,
                        SitioWeb = nuevaMarca.SitioWeb!,
                        Descripcion = nuevaMarca.Descripcion!,
                        EsMarcaLujo = nuevaMarca.EsMarcaLujo,
                        FechaCreacion = nuevaMarca.FechaCreacion,
                        FechaActualizacion = nuevaMarca.FechaActualizacion ?? DateTime.UtcNow
                    };

                    return CreatedAtAction(
                        nameof(GetMarcaById),
                        new { id = nuevaMarca.Id },
                        new ApiResponse(
                            mensaje: "Marca creada exitosamente",
                            exito: true,
                            datos: _formatterService.FormatMarca(marcaDtoResponse)
                        ));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear la marca {@MarcaDto}", marcaDto);
                    throw;
                }
            }
        }
    }