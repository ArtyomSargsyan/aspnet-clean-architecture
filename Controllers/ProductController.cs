using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ToDoApi.DTO;
using ToDoApi.Services.Products;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/admin/products")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService service, ILogger<ProductsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var items = await _service.GetAllProducts();
            _logger.LogInformation("Fetched {count} products-test", items.Count());
            return Ok(items);
        }

        [HttpGet("paged")]
        [ProducesResponseType(typeof(PagedResultDto<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetPagedProductsAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("summaries")]
        [ProducesResponseType(typeof(IEnumerable<ProductSmallDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductSummaries()
        {
            var items = await _service.GetProductSummariesAsync();
            return Ok(items);
        }

        [HttpGet("names-prices")]
        [ProducesResponseType(typeof(IEnumerable<ProductSmallDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductNamesAndPricesAsync()
        {
            var items = await _service.GetProductNamesAndPricesAsync();
            return Ok(items);
        }

        [HttpGet("count-per-category")]
        [ProducesResponseType(typeof(IEnumerable<CategoryProductCountDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductCountPerCategory()
        {
            var items = await _service.GetProductCountPerCategory();
            return Ok(items);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _service.GetProductByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] ProductCreateDto dto)
        {
            var created = await _service.CreateAsync(dto);
            _logger.LogInformation("Product created with Id {id}", created.Id);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromForm] ProductCreateDto dto)
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
