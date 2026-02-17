using MediatR;
namespace ToDoApi.Repositories.Pages;
using ToDoApi.Models;   

public class CreatePageHandler 
    : IRequestHandler<CreatePageCommand, int>
{
    private readonly IPageRepository _repo;

    public CreatePageHandler(IPageRepository repo)
    {
        _repo = repo;
    }

    public async Task<int> Handle(CreatePageCommand request, CancellationToken cancellationToken)
    {
        var page = new Page
        {
            Title = request.Title,
            Content = request.Content,
        };

        await _repo.AddAsync(page);
        return page.Id;
    }
}
