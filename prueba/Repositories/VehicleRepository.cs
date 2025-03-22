using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prueba.Data;
using prueba.Interfaces;
using prueba.Entities;
using Microsoft.EntityFrameworkCore;
using prueba.Dto;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using AutoMapper;
using prueba.Exceptions;
using prueba.Helpers;

namespace prueba.Repositories
{

    public class VehicleRepository : IVehicleRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<VehicleRepository> _logger;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;

        private readonly AppDbContext _context;



        public VehicleRepository(
            IUnitOfWork unitOfWork,
            ILogger<VehicleRepository> logger,
            IMemoryCache cache,
            IMapper mapper,
            AppDbContext context
            )
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _cache = cache;
            _mapper = mapper;
            _context = context;
        }

        // public async Task<IEnumerable<VehicleDTO>> ObtenerTodosAsync(string? term = null, int? year = null)
        // {
        //     string cacheKey = $"vehicles_{term}_{year}";

        //     return await _cache.GetOrCreateAsync(cacheKey, async entry =>
        //     {
        //         entry.SlidingExpiration = TimeSpan.FromMinutes(10);

        //         try
        //         {
        //             var query = _unitOfWork.Context.Vehiculos
        //                 .Include(v => v.Marca)
        //                 .AsNoTracking();

        //             if (!string.IsNullOrEmpty(term))
        //             {
        //                 query = query.Where(v =>
        //                     v.Modelo!.Contains(term) ||
        //                     (v.Marca != null && v.Marca!.Nombre!.Contains(term)));
        //             }

        //             if (year.HasValue)
        //             {
        //                 query = query.Where(v => v.Anio == year.Value);
        //             }

        //             var vehicles = await query.ToListAsync();
        //             List<VehicleDTO> vehiclesDto = _mapper.Map<List<VehicleDTO>>(vehicles) ?? new List<VehicleDTO>();
        //             return vehiclesDto;
        //         }
        //         catch (Exception ex)
        //         {
        //             _logger.LogError(ex, "Error retrieving vehicles");
        //             throw;
        //         }
        //     }) ?? new List<VehicleDTO>();
        // }

       public async Task<(IEnumerable<Vehiculo> vehicles, PaginationMetadata metadata)> ObtenerTodosAsync(
       PaginationParams paginationParams,
       string? term = null,
       int? year = null)
        {
            var query = _context.Vehiculos.AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                query = query.Where(v =>
                    v.Modelo!.ToLower().Contains(term) ||
                    v.Color!.ToLower().Contains(term) ||
                    v.Estado!.ToLower().Contains(term)
                );
            }

            if (year.HasValue)
            {
                query = query.Where(v => v.Anio == year.Value);
            }

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)paginationParams.PageSize);

            var metadata = new PaginationMetadata(
                pageNumber: paginationParams.PageNumber,
                pageSize: paginationParams.PageSize,
                totalItems: totalItems
            );

            var vehicles = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            return (vehicles, metadata);
        }


        public async Task<VehicleDTO> ObtenerPorIdAsync(int id)
        {
            var cacheKey = $"vehicle_{id}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
                       {
                           entry.SlidingExpiration = TimeSpan.FromMinutes(10);

                           try
                           {
                               var vehicle = await _unitOfWork.Context.Vehiculos
                        .Include(v => v.Marca)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(v => v.Id == id && v.Marca != null);

                               if (vehicle == null)
                               {
                                   _logger.LogWarning("Vehicle with ID {Id} not found", id);

                                   return null!;
                               }

                               var vehicleDto = _mapper.Map<VehicleDTO>(vehicle);

                               return vehicleDto;
                           }
                           catch (NotFoundException)
                           {
                               throw;
                           }
                           catch (Exception ex)
                           {
                               _logger.LogError(ex, "Error retrieving vehicle with ID {Id}", id);
                               throw;
                           }
                       }) ?? new VehicleDTO();
        }

        // Other methods with similar improvements...
        public async Task<IEnumerable<VehicleDTO>> ObtenerPorMarcaAsync(string marca)
        {
            string cacheKey = $"vehicles_marca_{marca}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(10);

                try
                {
                    var vehicles = await _unitOfWork.Context.Vehiculos
                        .Include(v => v.Marca)
                        .Where(v => v.Marca!.Nombre!.Contains(marca))
                        .AsNoTracking()
                        .ToListAsync();

                    return _mapper.Map<List<VehicleDTO>>(vehicles) ?? new List<VehicleDTO>();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving vehicles by marca {Marca}", marca);
                    throw;
                }
            }) ?? new List<VehicleDTO>();
        }

        public async Task<VehicleDTO> CrearAsync(Vehiculo vehiculo)
        {
            try
            {
                vehiculo.FechaCreacion = DateTime.Now;
                _unitOfWork.Context.Vehiculos.Add(vehiculo);
                await _unitOfWork.Complete();

                // Invalidar caché
                _cache.Remove("vehicles_");
                _cache.Remove($"vehicles_marca_");

                return _mapper.Map<VehicleDTO>(vehiculo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vehicle");
                throw;
            }
        }

        public async Task ActualizarAsync(VehicleDTO vehiculoDto)
        {
            try
            {
                var vehiculo = await _unitOfWork.Context.Vehiculos.FindAsync(vehiculoDto.Id);

                if (vehiculo == null)
                
                    throw new NotFoundException($"Vehicle with ID {vehiculoDto.Id} not found");

                _mapper.Map(vehiculoDto, vehiculo);
                
                vehiculo.FechaActualizacion = DateTime.Now;

                await _unitOfWork.Complete();

                // Invalidar caché
                _cache.Remove($"vehicle_{vehiculoDto.Id}");
                _cache.Remove("vehicles_");
                _cache.Remove($"vehicles_marca_");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vehicle");
                throw;
            }
        }

        public async Task EliminarAsync(int id)
        {
            try
            {
                var vehiculo = await _unitOfWork.Context.Vehiculos.FindAsync(id);
                if (vehiculo == null)
                    throw new NotFoundException($"Vehicle with ID {id} not found");

                _unitOfWork.Context.Vehiculos.Remove(vehiculo);
                await _unitOfWork.Complete();

                // Invalidar caché
                _cache.Remove($"vehicle_{id}");
                _cache.Remove("vehicles_");
                _cache.Remove($"vehicles_marca_");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vehicle with ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> ExisteMarcaAsync(int marcaId)
        {
            string cacheKey = $"marca_exists_{marcaId}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(10);

                try
                {
                    return await _unitOfWork.Context.Marcas.AnyAsync(m => m.Id == marcaId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error checking if marca exists with ID {MarcaId}", marcaId);
                    throw;
                }
            });
        }


    }
}