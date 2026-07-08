using BookStore.Api.Models;
using BookStore.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookService _bookService;

    public BooksController(BookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_bookService.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var book = _bookService.GetById(id);

        if (book is null)
            return NotFound();

        return Ok(book);
    }

    [HttpPost]
    public IActionResult Create(Book book)
    {
        var createdBook = _bookService.Create(book);

        return CreatedAtAction(nameof(GetById),
            new { id = createdBook.Id },
            createdBook);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Book book)
    {
        var updated = _bookService.Update(id, book);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _bookService.Delete(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}