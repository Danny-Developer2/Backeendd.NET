using prueba.Dto;
using prueba.Helpers;

namespace prueba.Services
{
    public interface IVehicleFormatterService
    {
        object FormatVehicle(VehicleDTO vehicle);
    }

    public class VehicleFormatterService : IVehicleFormatterService
    {
        public object FormatVehicle(VehicleDTO vehicle)
        {
            return new
            {
                id = vehicle.Id,
                modelo = vehicle.Modelo,
                marca = vehicle.Marca?.Nombre,
                anio = vehicle.Anio,
                color = vehicle.Color,
                precio = $"${vehicle.Precio:N2}",
                transmision = VehicleHerlper.FormatearTransmision(vehicle.Transmision!),
                tipoCombustible = VehicleHerlper.FormatearCombustible(vehicle.TipoCombustible!),
                kilometraje = VehicleHerlper.FormatearKilometraje(vehicle.Kilometraje),
                numeroChasis = vehicle.NumeroChasis
            };
        }
    }
}