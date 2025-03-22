using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using prueba.Controllers;

namespace prueba.Entities
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public string? Modelo { get; set; }
        public int MarcaId { get; set; }
        public int Anio { get; set; }
        public string? Color { get; set; }
        public decimal Precio { get; set; }
        public string? Transmision { get; set; }
        public string? TipoCombustible { get; set; }
        public int Kilometraje { get; set; }
        public string? NumeroChasis { get; set; }
        public string? Estado { get; set; }
        public int NumeroPuertas { get; set; }
        public int Capacidad { get; set; }
        public string? UrlImagen { get; set; }
        public bool Disponible { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

        [JsonIgnore]
        public virtual Marca? Marca { get; set; }
    }
}
