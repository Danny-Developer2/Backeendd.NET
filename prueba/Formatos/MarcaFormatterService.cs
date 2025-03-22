using prueba.Dto;
using prueba.Helpers;

namespace prueba.Formatos
{
    public interface IMarcaFormatterService
    {
        object FormatMarca(MarcaDTO marca);
    }

    public class MarcaFormatterService : IMarcaFormatterService
    {
        public object FormatMarca(MarcaDTO marca)
        {
            return new
            {
                id = marca.Id,
                nombre = MarcaHelper.FormatearNombreCompleto(marca.Nombre, marca.Pais),
                pais = marca.Pais,
                anioFundacion = marca.AnioFundacion,
                sedeCentral = MarcaHelper.FormatearSede(marca.SedeCentral, marca.Pais),
                urlLogo = marca.UrlLogo,
                sitioWeb = marca.SitioWeb,
                descripcion = marca.Descripcion,
                esMarcaLujo = MarcaHelper.FormatearTipoMarca(marca.EsMarcaLujo),
                fechaCreacion = marca.FechaCreacion,
                fechaActualizacion = marca.FechaActualizacion
            };
        }
    }
}


//  public object FormatVehicle(VehicleDTO vehicle)
//         {
//             return new
//             {
//                 id = vehicle.Id,
//                 modelo = vehicle.Modelo,
//                 marca = vehicle.Marca?.Nombre,
//                 anio = vehicle.Anio,
//                 color = vehicle.Color,
//                 precio = $"${vehicle.Precio:N2}",
//                 transmision = VehicleHerlper.FormatearTransmision(vehicle.Transmision!),
//                 tipoCombustible = VehicleHerlper.FormatearCombustible(vehicle.TipoCombustible!),
//                 kilometraje = VehicleHerlper.FormatearKilometraje(vehicle.Kilometraje),
//                 numeroChasis = vehicle.NumeroChasis
//             };
//         }