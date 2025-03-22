using Microsoft.AspNetCore.Mvc;
using prueba.Entities;
using prueba.Interfaces;


namespace prueba.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoRepository _repo;

        public ProductosController(IProductoRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
        {
            var productos = await _repo.ObtenerTodosAsync();
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var productos = await _repo.ObtenerTodosAsync();
            var producto = productos.FirstOrDefault(p => p.Id == id);
            if (producto == null)
            {
                return NotFound(new{
                    message = "Producto no encontrado",
                    code = 404
                });
            }
            return Ok(new
            {
                message = "Producto encontrado exitosamente",
                data = producto
            });
        }
    }
}