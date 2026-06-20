using ToDoApi.DTO;
using ToDoApi.Models;

namespace ToDoApi.Services.Products;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProducts();
    Task<PagedResultDto<ProductDto>> GetPagedProductsAsync(int page, int pageSize);
    Task<IEnumerable<ProductSmallDto>> GetProductSummariesAsync();
    Task<IEnumerable<ProductSmallDto>> GetProductNamesAndPricesAsync();
    Task<IEnumerable<CategoryProductCountDto>> GetProductCountPerCategory();
    Task<ProductDto?> GetProductByIdAsync(int id);
    Task<ProductDto> CreateAsync(ProductCreateDto dto);
    Task<ProductDto?> UpdateAsync(int id, ProductCreateDto dto);
    Task<bool> DeleteAsync(int id);
}
