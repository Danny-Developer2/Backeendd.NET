using prueba.Entities;

namespace prueba.Extensions
{
    public static class MarcaExtensions
    {
        public static int ObtenerAntiguedad(this Marca marca)
        {
            return DateTime.Now.Year - marca.AnioFundacion;
        }

        public static string ObtenerUbicacionCompleta(this Marca marca)
        {
            return $"{marca.SedeCentral}, {marca.Pais}";
        }

        public static string ObtenerCategoria(this Marca marca)
        {
            return marca.EsMarcaLujo ? "Premium" : "Standard";
        }
    }
}