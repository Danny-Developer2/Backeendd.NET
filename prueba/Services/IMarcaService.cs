using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prueba.Dto;
using prueba.Helpers;

namespace prueba.Services
{
    public interface IMarcaService
    {
        Task<(IEnumerable<MarcaDTO> marcas, PaginationMetadata metadata)> GetMarcasAsync(PaginationParams paginationParams);
        Task<MarcaDTO?> GetMarcaByIdAsync(int id);
        Task<MarcaDTO> CreateMarcaAsync(MarcaDTO marcaDto);
        Task<bool> ValidateMarcaAsync(MarcaDTO marcaDto);
    }
}