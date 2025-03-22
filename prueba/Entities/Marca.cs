using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace prueba.Entities
{
    public class Marca
    {
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Pais { get; set; }
    public int AnioFundacion { get; set; }
    public string? SedeCentral { get; set; }
    public string? UrlLogo { get; set; }
    public string? SitioWeb { get; set; }
    public string? Descripcion { get; set; }
    public bool EsMarcaLujo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }

    [JsonIgnore]
    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
        
    }
}