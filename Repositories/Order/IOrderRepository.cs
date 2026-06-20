using ToDoApi.Domain;

namespace ToDoApi.Repositories;

public interface IOrderRepository
{
    Task AddAsync(Order order);
    Task SaveChangesAsync();
}