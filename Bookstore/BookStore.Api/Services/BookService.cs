using BookStore.Api.Models;
using BookStore.Api.Rules;

namespace BookStore.Api.Services;

public class BookService
{
    private static readonly List<Book> _books =
    [
        new()
        {
            Id = 1,
            Title = "Clean Code",
            Author = "Robert C. Martin",
            ISBN = "9780132350884",
            PublishedDate = new DateTime(2008, 8, 1),
            Price = 49.99m,
            Stock = 12
        },
        new()
        {
            Id = 2,
            Title = "Clean Architecture",
            Author = "Robert C. Martin",
            ISBN = "9780134494166",
            PublishedDate = new DateTime(2017, 9, 20),
            Price = 59.99m,
            Stock = 8
        },
        new() {
            Id = 3,
            Title = "Clean Code",
            Author = "Robert C. Martin",
            ISBN = "9780132350884",
            PublishedDate = new DateTime(2008, 8, 1),
            Price = 49.99m,
            Stock = 12,
            IsPremium = true,
            IsLoaned = false
        }
    ];

    private readonly IEnumerable<IBookDeletionRule> _deletionRules;

    public BookService(IEnumerable<IBookDeletionRule> deletionRules)
    {
        _deletionRules = deletionRules;
    }

    public IEnumerable<Book> GetAll()
    {
        return _books;
    }

    public Book? GetById(int id)
    {
        return _books.FirstOrDefault(b => b.Id == id);
    }

    public Book Create(Book book)
    {
        book.Id = _books.Max(b => b.Id) + 1;

        _books.Add(book);

        return book;
    }

    public bool Update(int id, Book updatedBook)
    {
        var book = GetById(id);

        if (book is null)
            return false;

        book.Title = updatedBook.Title;
        book.Author = updatedBook.Author;
        book.ISBN = updatedBook.ISBN;
        book.Price = updatedBook.Price;
        book.Stock = updatedBook.Stock;
        book.PublishedDate = updatedBook.PublishedDate;

        return true;
    }

    public bool Delete(int id)
    {
        var book = GetById(id);

        if (book is null)
            return false;

        foreach (var rule in _deletionRules)
        {
            rule.Validate(book);
        }

        _books.Remove(book);

        return true;
    }
}