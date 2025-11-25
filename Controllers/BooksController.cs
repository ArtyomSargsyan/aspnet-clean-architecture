using Microsoft.AspNetCore.Mvc;
using ToDoApi.Domain;
using ToDoApi.Infrastructure.Storage;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookStore _bookStore;

        public BooksController(IBookStore bookStore)
        {
            _bookStore = bookStore;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_bookStore.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var book = _bookStore.GetById(id);
            return book == null ? NotFound() : Ok(book);
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            _bookStore.Add(book);
            return Ok(book);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Book book)
        {
            var updated = _bookStore.Update(id, book);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _bookStore.Delete(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
