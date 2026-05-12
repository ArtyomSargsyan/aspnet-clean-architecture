namespace ToDoApi.Exceptions;

public sealed class ConflictException(string message) : AppException(message);
