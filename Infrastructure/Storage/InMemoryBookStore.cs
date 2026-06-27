using System.Collections.Concurrent;
using ToDoApi.Domain;

namespace ToDoApi.Infrastructure.Storage;

public class InMemoryBookStore : IBookStore
{
    private readonly ConcurrentDictionary<int, Book> _books = new();
    private int _nextId = 0;

    public IEnumerable<Book> GetAll() => _books.Values;

    public Book? GetById(int id)
    {
        _books.TryGetValue(id, out var book);
        return book;
    }

    public void Add(Book book)
    {
        book.Id = Interlocked.Increment(ref _nextId);

        if (!_books.TryAdd(book.Id, book))
            throw new InvalidOperationException($"Book with ID {book.Id} already exists.");
    }

    public bool Update(int id, Book book)
    {
        if (!_books.ContainsKey(id))
            return false;

        _books[id] = book;
        return true;
    }

    public bool Delete(int id) => _books.TryRemove(id, out _);
}
