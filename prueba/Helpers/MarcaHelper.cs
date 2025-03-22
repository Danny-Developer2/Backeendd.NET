using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prueba.Helpers
{
    public static class MarcaHelper
    {
        public static string FormatearNombreCompleto(string nombre, string pais)
        {
            return $"{nombre} ({pais})";
        }

        public static string FormatearAntiguedad(int anioFundacion)
        {
            int antiguedad = DateTime.Now.Year - anioFundacion;
            return $"{antiguedad} años de historia";
        }

        public static string FormatearTipoMarca(bool esMarcaLujo)
        {
            return esMarcaLujo ? "Marca de Lujo" : "Marca Regular";
        }

        public static string FormatearSede(string sedeCentral, string pais)
        {
            return $"Sede Central: {sedeCentral}, {pais}";
        }

    }
}