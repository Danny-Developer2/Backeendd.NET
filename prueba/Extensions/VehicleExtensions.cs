using prueba.Entities;

namespace prueba.Extensions
{
    public static class VehiculoExtensions
    {
        public static bool EsNuevo(this Vehiculo vehiculo)
        {
            return vehiculo.Kilometraje < 1000;
        }

        public static bool RequiereMantenimiento(this Vehiculo vehiculo)
        {
            return vehiculo.Kilometraje > 10000;
        }

        public static string ObtenerDescripcionCompleta(this Vehiculo vehiculo)
        {
            return $"{vehiculo.Marca?.Nombre} {vehiculo.Modelo} ({vehiculo.Anio})";
        }
    }
}