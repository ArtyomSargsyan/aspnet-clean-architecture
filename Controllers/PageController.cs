using MediatR;
using Microsoft.AspNetCore.Mvc;
using TODOAPI.CQRS.Pages.Queries;

[ApiController]
[Route("api/pages")]
public class PagesController : ControllerBase
{
    private readonly IMediator _mediator;

    public PagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllPagesQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePageCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { Id = id });
    }
}
