using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prueba.Helpers
{
    public static class VehicleHerlper
    {

        public static string FormatearKilometraje(int kilometraje)
        {
            return $"{kilometraje:N0} km";
        }

        public static string FormatearModelo(string marca, string modelo, int anio)
        {
            return $"{marca} {modelo} ({anio})";
        }

        public static string FormatearTransmision(string transmision)
        {
            return transmision.ToUpper() switch
            {
                "A" => "Automática",
                "M" => "Manual",
                "CVT" => "CVT",
                _ => transmision
            };
        }

        public static string FormatearCombustible(string tipoCombustible)
        {
            return tipoCombustible.ToUpper() switch
            {
                "G" => "Gasolina",
                "D" => "Diésel",
                "E" => "Eléctrico",
                "H" => "Híbrido",
                _ => tipoCombustible
            };
        }

    }
}