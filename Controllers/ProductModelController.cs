using Microsoft.AspNetCore.Mvc;
using ToDoApi.Services.ProductModel;
using ToDoApi.DTO;

namespace ToDoApi.Controller
{
    public class ProductModelController : ControllerBase
    {
       protected readonly ILogger<ProductModelController> _logger;
       protected readonly IProductModelService _service;
       public ProductModelController(ILogger<ProductModelController> logger, IProductModelService service)
       {
            _logger = logger;
            _service = service;
       }

        [HttpGet("/api/product-models")]
        public async Task<ActionResult<IEnumerable<ProductModelDto>>> GetAll()
        {
            var items = await _service.GetAllAsync();   
            return Ok(items);
        }

        [HttpGet("product-models/{id}")]
        public async Task<ActionResult<ProductModelDto>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost("/api/product-models")]
        public async Task<IActionResult> Create([FromBody] CreateProductModelDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("product-models/{id}")]
        public async Task<ActionResult<ProductModelDto>> Update(int id, UpdateProductModelDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null)
                return NotFound($"ProductModel with ID {id} not found.");

            return Ok(updated);
        }

        [HttpDelete("product-models/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound($"ProductModel with ID {id} not found.");

            return NoContent();
        }
    }
}