using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using prueba.Entities;

namespace prueba.Dto
{
    public class VehicleDTO
    {
        public int Id { get; set; } 
        public string Modelo { get; set; } = string.Empty;
        public int MarcaId { get; set; }
        public int Anio { get; set; }
        public string Color { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public string Transmision { get; set; } = string.Empty;
        public string TipoCombustible { get; set; } = string.Empty;
        public int Kilometraje { get; set; }
        public string NumeroChasis { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public int NumeroPuertas { get; set; }
        public int Capacidad { get; set; }
        public string UrlImagen { get; set; } = string.Empty;
        public bool Disponible { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; } = DateTime.Now;

        [JsonIgnore]
        public virtual Marca? Marca { get; set; }

        // public virtual Marca? Marca { get; set; }
    }
}