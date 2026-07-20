using BookStore.Api.Data;
using BookStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Api.Repositories;

public class BookRepository(BookStoreDbContext context) : IBookRepository
{
    private readonly BookStoreDbContext _context = context;

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await _context.Books
            .AsNoTracking()
            .Include(b => b.Author)
            .ToListAsync();
    }

    public async Task<Book?> GetByIdAsync(int id)
    {
        return await _context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task AddAsync(Book book)
    {
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Book book)
    {
        var existingBook = await _context.Books.FindAsync(book.Id);

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

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book is null)
            return;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }
}