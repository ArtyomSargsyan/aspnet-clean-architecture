using ToDoApi.Models;
using ToDoApi.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ToDoApi.Repositories.Products
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
         Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(int page, int pageSize);
        Task<IEnumerable<ProductSmallDto>> GetProductSummariesAsync();
        Task<IEnumerable<ProductSmallDto>> GetProductNamesAndPricesAsync();
        Task<List<CategoryProductCountDto>> GetProductCountPerCategory();
        Task<Product?> GetByIdAsync(int id);
        Task<Product> AddAsync(Product product);
        Task<Product?> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
    }
}
