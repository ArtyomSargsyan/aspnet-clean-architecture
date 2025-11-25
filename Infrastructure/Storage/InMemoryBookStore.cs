using System.Collections.Concurrent;
using ToDoApi.Domain;

namespace ToDoApi.Infrastructure.Storage
{
    public class InMemoryBookStore : IBookStore
    {
        private readonly ConcurrentDictionary<int, Book> _books = new();

        public IEnumerable<Book> GetAll()
        {
            return _books.Values;
        }

        public Book? GetById(int id)
        {
            _books.TryGetValue(id, out var book);
            return book;
        }

        public void Add(Book book)
        {
            if (!_books.TryAdd(book.Id, book))
                throw new Exception($"Book with ID {book.Id} already exists.");
        }

        public bool Update(int id, Book book)
        {
            if (!_books.ContainsKey(id))
                return false;

            _books[id] = book;
            return true;
        }

        public bool Delete(int id)
        {
            return _books.TryRemove(id, out _);
        }
    }
}
