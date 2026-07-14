
using BookStore.Api.Models;

namespace BookStore.Api.Repositories;

public class BookRepository : IRepository<Book>
{
    private readonly List<Book> _books = [];

    public IEnumerable<Book> GetAll()
    {
        return _books;
    }

    public Book? GetById(int id)
    {
        return _books.FirstOrDefault(b => b.Id == id);
    }

    public void Add(Book book)
    {
        _books.Add(book);
    }

    public void Update(Book book)
    {
        var existingBook = GetById(book.Id);

        if (existingBook is null)
            return;

        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.IsPremium = book.IsPremium;
        existingBook.IsLoaned = book.IsLoaned;
        existingBook.IsHistorical = book.IsHistorical;
        existingBook.IsArchived = book.IsArchived;
    }

    public void Delete(int id)
    {
        var book = GetById(id);

        if (book is not null)
        {
            _books.Remove(book);
        }
    }
}