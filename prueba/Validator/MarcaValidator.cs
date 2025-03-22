using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.AspNetCore;
using prueba.Entities;

namespace prueba.Validator
{
    public class MarcaValidator: AbstractValidator<Marca>
    {
        // TODO: Implementar validaciones específicas para la marca
        // Ejemplos:
        // RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es obligatorio")
        // RuleFor(x => x.Pais).NotEmpty().WithMessage("El país es obligatorio")
        // RuleFor(x => x.AnioFundacion).GreaterThan(1900).WithMessage("El año de fundación debe ser mayor a 1900")
        //...
        public MarcaValidator()
        {
        RuleFor(x => x.Nombre).NotEmpty().WithMessage("El nombre es obligatorio");
        RuleFor(x => x.Pais).NotEmpty().WithMessage("El país es obligatorio");
        RuleFor(x => x.AnioFundacion).GreaterThan(1900).WithMessage("El año de fundación debe ser mayor a 1900");
        RuleFor(x => x.SedeCentral).NotEmpty().WithMessage("La sede central es obligatoria");
        RuleFor(x => x.UrlLogo).NotEmpty().WithMessage("La URL del logo es obligatoria");
        RuleFor(x => x.SitioWeb).NotEmpty().WithMessage("El sitio web es obligatorio");
        RuleFor(x => x.Descripcion).NotEmpty().WithMessage("La descripción es obligatoria");
        RuleFor(x => x.EsMarcaLujo).NotEmpty().WithMessage("La marca de lujo es obligatoria");
        RuleFor(x => x.FechaCreacion).NotEmpty().WithMessage("La fecha de creación es obligatoria");
        RuleFor(x => x.FechaActualizacion).NotEmpty().WithMessage("La fecha de actualización es obligatoria");
            
        }
    


    }
}





// public string Nombre { get; set; } = string.Empty;
// public string Pais { get; set; } = string.Empty;
// public int AnioFundacion { get; set; }
// public string SedeCentral { get; set; } = string.Empty;
// public string UrlLogo { get; set; } = string.Empty;
// public string SitioWeb { get; set; } = string.Empty;
// public string Descripcion { get; set; } = string.Empty;
// public bool EsMarcaLujo { get; set; }
// public DateTime FechaCreacion { get; set; }
// public DateTime FechaActualizacion { get; set; } = DateTime.Now;