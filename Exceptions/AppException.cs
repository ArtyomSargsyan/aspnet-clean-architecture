namespace ToDoApi.Exceptions;

/// <summary>
/// Base class for all domain-level exceptions.
/// Caught by the global handler and mapped to an appropriate HTTP response.
/// </summary>
public abstract class AppException(string message) : Exception(message);
