using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoApi.Features.Articles.Commands;
using ToDoApi.Features.Articles.Queries;

namespace ToDoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArticleController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArticleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArticleCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var articles = await _mediator.Send(new GetAllArticlesQuery());
        return Ok(articles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var article = await _mediator.Send(new GetArticleByIdQuery(id));
        return Ok(article);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteArticleCommand(id));
        return NoContent();
    }
}
