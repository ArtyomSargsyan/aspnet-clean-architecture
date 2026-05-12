namespace ToDoApi.Exceptions;

public sealed class NotFoundException(string resourceName, object key)
    : AppException($"{resourceName} with key '{key}' was not found.");
