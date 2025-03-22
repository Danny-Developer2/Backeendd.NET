using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prueba.Dto
{
    public class MarcaDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public int AnioFundacion { get; set; }
        public string SedeCentral { get; set; } = string.Empty;
        public string UrlLogo { get; set; } = string.Empty;
        public string SitioWeb { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool EsMarcaLujo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;
    }
}