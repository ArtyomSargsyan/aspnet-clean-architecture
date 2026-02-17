using MediatR;

public record CreatePageCommand(string Title, string Content) : IRequest<int>;
