using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using prueba.Data;
using prueba.Entities;
using prueba.Helpers;
using prueba.Interfaces;

namespace prueba.Repositories
{
    public class MarcaRepository : IMarcaRespository
    {
        private readonly AppDbContext _context;

        public MarcaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Marca> marcas, PaginationMetadata metadata)> ObtenerTodosAsync(PaginationParams paginationParams)
        {
            var query = _context.Marcas.AsQueryable();

            var totalItems = await query.CountAsync();
            var metadata = new PaginationMetadata(
                paginationParams.PageNumber,
                paginationParams.PageSize,
                totalItems
            );

            var marcas = await query
                .OrderBy(m => m.Id)
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            return (marcas, metadata);
        }


        public async Task<Marca> CrearAsync(Marca marca)
        {
            marca.FechaCreacion = DateTime.Now;
            _context.Marcas.Add(marca);
            await _context.SaveChangesAsync();
            return marca;
        }

        public async Task<bool> TieneVehiculosAsociadosAsync(int marcaId)
        {
            return await _context.Vehiculos.AnyAsync(v => v.MarcaId == marcaId);
        }

        public async Task<Marca> ObtenerPorIdAsync(int id)
        {


            var marca = await _context.Marcas.FindAsync(id);
            if (marca == null)
            {
                return null!;
            }
            return marca;
        }

        public async Task ActualizarAsync(Marca marca)
        {
            marca.FechaActualizacion = DateTime.Now;
            _context.Entry(marca).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var marca = await _context.Marcas.FindAsync(id);
            if (marca != null)
            {
                _context.Marcas.Remove(marca);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistePorNombreAsync(string nombre)
        {
            return await _context.Marcas.AnyAsync(m => m.Nombre == nombre);
        }
        public async Task<IEnumerable<Marca>> ObtenerPorUrlLogoAsync(string urlLogo)
        {
            return await _context.Marcas.Where(m => m.UrlLogo == urlLogo).ToListAsync();
        }
        public async Task<IEnumerable<Marca>> ObtenerPorPaisAsync(string pais)
        {
            return await _context.Marcas.Where(m => m.Pais == pais).ToListAsync();
        }
    }
}