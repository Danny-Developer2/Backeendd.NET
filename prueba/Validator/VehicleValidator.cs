using FluentValidation;
using prueba.Entities;

namespace prueba.Validators
{
    public class VehicleValidator : AbstractValidator<Vehiculo>
    {


        public VehicleValidator()
        {
            RuleFor(x => x.Modelo)
                .NotEmpty().WithMessage("El modelo es obligatorio")
                .MaximumLength(100).WithMessage("El modelo no puede exceder los 100 caracteres");

            RuleFor(x => x.MarcaId)
                .GreaterThan(0).WithMessage("La marca es obligatoria");

            RuleFor(x => x.Anio)
                .GreaterThan(1900).WithMessage("El año debe ser mayor a 1900")
                .LessThanOrEqualTo(DateTime.Now.Year + 1).WithMessage("El año no puede ser futuro");

            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("El color es obligatorio")
                .MaximumLength(50).WithMessage("El color no puede exceder los 50 caracteres");

            RuleFor(x => x.Precio)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0");

            RuleFor(x => x.UrlImagen)
                .NotEmpty().WithMessage("La URL de la imagen es obligatoria")
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .WithMessage("La URL de la imagen no es válida");

            RuleFor(x => x.Transmision)
                .NotEmpty().WithMessage("La transmisión es obligatoria")
                .IsInEnum().WithMessage("Tipo de transmisión no válido");

            RuleFor(x => x.TipoCombustible)
                .NotEmpty().WithMessage("El tipo de combustible es obligatorio")
                .IsInEnum().WithMessage("Tipo de combustible no válido");

            RuleFor(x => x.Kilometraje)
                .GreaterThanOrEqualTo(0).WithMessage("El kilometraje no puede ser negativo");

            RuleFor(x => x.NumeroChasis)
                .NotEmpty().WithMessage("El número de chasis es obligatorio")
                .Length(17).WithMessage("El número de chasis debe tener 17 caracteres")
                .Matches(@"^[A-HJ-NPR-Z0-9]{17}$").WithMessage("Formato de número de chasis no válido");
        }

    }
}