using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prueba.Dto;
using prueba.Entities;
using prueba.Helpers;

namespace prueba.Interfaces
{
    public interface IVehicleRepository
    {
       Task<(IEnumerable<Vehiculo> vehicles, PaginationMetadata metadata)> ObtenerTodosAsync(
        PaginationParams paginationParams, 
        string? term = null, 
        int? year = null);

        Task<VehicleDTO> ObtenerPorIdAsync(int id);

        Task<VehicleDTO> CrearAsync(Vehiculo vehiculo);

        Task ActualizarAsync(VehicleDTO vehiculo);

        Task EliminarAsync(int id);

       
        Task<IEnumerable<VehicleDTO>> ObtenerPorMarcaAsync(string marca);

        Task<bool> ExisteMarcaAsync(int marcaId);
        
    }
}