using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prueba.Interfaces;

namespace prueba.Middleware
{
    public class MarcaMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMarcaRespository _marcaRepository;

        public MarcaMiddleware(RequestDelegate next, IMarcaRespository marcaRepository)
        {
            _next = next;
            _marcaRepository = marcaRepository;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value?.ToLower() ?? "";

            if (path.StartsWith("/api/marcas"))
            {
                if (context.Request.Method == "DELETE")
                {
                    int marcaId;
                    if (int.TryParse(context.Request.Path.Value!.Split('/').Last(), out marcaId))
                    {
                        // Verificar si hay vehículos asociados a esta marca
                        var tieneVehiculos = await _marcaRepository.TieneVehiculosAsociadosAsync(marcaId);
                        if (tieneVehiculos)
                        {
                            context.Response.StatusCode = 400;
                            await context.Response.WriteAsJsonAsync(new
                            {
                                mensaje = "No se puede eliminar la marca porque tiene vehículos asociados"
                            });
                            return;
                        }
                    }
                }
            }

            await _next(context);
        }

    }
}