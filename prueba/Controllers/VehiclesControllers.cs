using Microsoft.AspNetCore.Mvc;
using prueba.Services;
using prueba.Dto;
using prueba.Entities;
using prueba.Errors;
using prueba.Interfaces;
using prueba.Helpers;

namespace prueba.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public class VehiclesController : ApiControllerBase
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehicleFormatterService _formatter;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(
            IVehicleRepository vehicleRepository,
            IVehicleFormatterService formatter,
            ILogger<VehiclesController> logger)
        {
            _vehicleRepository = vehicleRepository;
            _formatter = formatter;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetVehicles(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string? term,
            [FromQuery] int? year)
        {
            try
            {
                if (!ModelState.IsValid)
                    return HandleBadRequest("Parámetros de paginación inválidos");

                var (vehicles, metadata) = await _vehicleRepository.ObtenerTodosAsync(paginationParams, term, year);

                if (!vehicles.Any())
                    return HandleNotFound("No se encontraron vehículos");

                var vehiculosFormateados = vehicles.Select(v => _formatter.FormatVehicle(new VehicleDTO
                {
                    Id = v.Id,
                    MarcaId = v.MarcaId,
                    Modelo = v.Modelo!,
                    Anio = v.Anio,
                    Color = v.Color!,
                    Kilometraje = v.Kilometraje,
                    Precio = v.Precio,
                    Transmision = v.Transmision!,
                    TipoCombustible = v.TipoCombustible!,
                    NumeroChasis = v.NumeroChasis!,
                    Estado = v.Estado!,
                    NumeroPuertas = v.NumeroPuertas,
                    Capacidad = v.Capacidad,
                    UrlImagen = v.UrlImagen!,
                    Disponible = v.Disponible,
                    FechaCreacion = v.FechaCreacion,
                    FechaActualizacion = v.FechaActualizacion ?? DateTime.UtcNow
                }));

                var response = new { pagination = metadata, data = vehiculosFormateados };

                return HandleSuccess("Vehículos obtenidos exitosamente", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo vehículos. Términos: {@SearchParams}",
                    new { term, year, paginationParams });
                throw;
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetVehicleById(int id)
        {
            try
            {
                if (id <= 0)
                    return HandleBadRequest("ID inválido");

                var vehicle = await _vehicleRepository.ObtenerPorIdAsync(id);

                if (vehicle == null || vehicle.Id == 0)
                    return HandleNotFound($"No se encontró el vehículo con ID {id}");

                return HandleSuccess("Vehículo obtenido exitosamente",
                    _formatter.FormatVehicle(vehicle));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener vehículo con ID {Id}", id);
                throw;
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse>> CreateVehicle([FromBody] VehicleDTO vehiculoDto)
        {
            try
            {
                if (vehiculoDto.Precio <= 0){
                    return HandleBadRequest("El precio del vehículo no puede ser menor o igual a 0");
                }
                
                if (!ModelState.IsValid)
                    return HandleBadRequest("Datos del vehículo inválidos");

                if (!await _vehicleRepository.ExisteMarcaAsync(vehiculoDto.MarcaId))
                    return HandleBadRequest($"La marca con ID {vehiculoDto.MarcaId} no existe");

                var vehiculo = new Vehiculo
                {
                    MarcaId = vehiculoDto.MarcaId,
                    Modelo = vehiculoDto.Modelo,
                    Anio = vehiculoDto.Anio,
                    Color = vehiculoDto.Color,
                    Kilometraje = vehiculoDto.Kilometraje,
                    Precio = vehiculoDto.Precio,
                    Transmision = vehiculoDto.Transmision,
                    TipoCombustible = vehiculoDto.TipoCombustible,
                    NumeroChasis = vehiculoDto.NumeroChasis,
                    Estado = vehiculoDto.Estado,
                    NumeroPuertas = vehiculoDto.NumeroPuertas,
                    Capacidad = vehiculoDto.Capacidad,
                    UrlImagen = vehiculoDto.UrlImagen,
                    Disponible = vehiculoDto.Disponible,
                    FechaCreacion = DateTime.UtcNow
                };

                var nuevoVehiculo = await _vehicleRepository.CrearAsync(vehiculo);

                return CreatedAtAction(
                    nameof(GetVehicleById),
                    new { id = nuevoVehiculo.Id },
                    new ApiResponse(
                        mensaje: "Vehículo creado exitosamente",
                        exito: true,
                        datos: _formatter.FormatVehicle(nuevoVehiculo)
                    )
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear vehículo {@VehicleDto}", vehiculoDto);
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse>> UpdateVehicle(int id, [FromBody] VehicleDTO vehiculoDto)
        {
            try
            {
                if (id <= 0)
                    return HandleBadRequest("ID inválido");

                if (id != vehiculoDto.Id)
                    return HandleBadRequest("El ID en la URL no coincide con el ID del vehículo");

                if (!await _vehicleRepository.ExisteMarcaAsync(vehiculoDto.MarcaId))
                    return HandleBadRequest($"La marca con ID {vehiculoDto.MarcaId} no existe");

                var existingVehicle = await _vehicleRepository.ObtenerPorIdAsync(id);

                if (existingVehicle == null || existingVehicle.Id == 0) 
                    return HandleNotFound($"No se encontró el vehículo con ID {id}");

                vehiculoDto.FechaActualizacion = DateTime.UtcNow;

                await _vehicleRepository.ActualizarAsync(vehiculoDto);

                var updatedVehicle = await _vehicleRepository.ObtenerPorIdAsync(id);

                return HandleSuccess("Vehículo actualizado exitosamente",
                    _formatter.FormatVehicle(updatedVehicle!));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar vehículo. ID: {Id}, Detalles: {@Vehicle}",
                    id, vehiculoDto);
                throw;
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> DeleteVehicle(int id)
        {
            try
            {
                if (id <= 0)
                    return HandleBadRequest("ID de vehículo inválido");

                var vehiculo = await _vehicleRepository.ObtenerPorIdAsync(id);

                if (vehiculo == null || vehiculo.Id == 0)
                    return HandleNotFound($"No se encontró el vehículo con ID {id}");

                await _vehicleRepository.EliminarAsync(id);

                return HandleSuccess("Vehículo eliminado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar vehículo. ID: {Id}", id);
                throw;
            }
        }
    }
}