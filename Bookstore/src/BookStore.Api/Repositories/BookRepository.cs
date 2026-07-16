using BookStore.Api.Data;
using BookStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Repositories;

public class BookRepository(BookStoreDbContext context) : IRepository<Book>
{
    private readonly BookStoreDbContext _context = context;

    public IEnumerable<Book> GetAll()
    {
        return _context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .ToList();
    }

    public Book? GetById(int id)
    {
        return _context.Books
            .Include(b => b.Author)
            .FirstOrDefault(b => b.Id == id);
    }

    public void Add(Book book)
    {
        _context.Books.Add(book);
        _context.SaveChanges();
    }

    public void Update(Book book)
    {
        var existingBook = _context.Books.Find(book.Id);

        if (existingBook is null)
            return;

        existingBook.Title = book.Title;
        existingBook.AuthorId = book.AuthorId;
        existingBook.ISBN = book.ISBN;
        existingBook.PublishedDate = book.PublishedDate;
        existingBook.Price = book.Price;
        existingBook.Stock = book.Stock;
        existingBook.IsPremium = book.IsPremium;
        existingBook.IsLoaned = book.IsLoaned;
        existingBook.IsHistorical = book.IsHistorical;
        existingBook.IsArchived = book.IsArchived;

        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var book = _context.Books.Find(id);

        if (book is null)
            return;

        _context.Books.Remove(book);
        _context.SaveChanges();
    }
}