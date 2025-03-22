using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prueba.Interfaces;


namespace prueba.Middleware
{
    public class VehicleMiddleware
    {

        private readonly RequestDelegate _next;
        private readonly IVehicleRepository _vehiculoRepository;

        public VehicleMiddleware(RequestDelegate next, IVehicleRepository vehiculoRepository)
        {
            _next = next;
            _vehiculoRepository = vehiculoRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value?.ToLower() ?? "";

            if (path.StartsWith("/api/vehiculos"))
            {
               
            }

            // Continuar con el siguiente middleware
            await _next(context);
        }

    }
}