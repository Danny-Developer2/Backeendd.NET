using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prueba.Entities;
using prueba.Helpers;

namespace prueba.Interfaces
{
    public interface IMarcaRespository
    {
         Task<(IEnumerable<Marca> marcas, PaginationMetadata metadata)> ObtenerTodosAsync(PaginationParams paginationParams);

        Task<Marca> ObtenerPorIdAsync(int id);

        Task<Marca> CrearAsync(Marca marca);

        Task ActualizarAsync(Marca marca);

        Task EliminarAsync(int id);

        Task<bool> ExistePorNombreAsync(string nombre);

        Task<IEnumerable<Marca>> ObtenerPorUrlLogoAsync(string urlLogo);

        Task<IEnumerable<Marca>> ObtenerPorPaisAsync(string pais);
        Task<bool> TieneVehiculosAsociadosAsync(int marcaId);
    }
}