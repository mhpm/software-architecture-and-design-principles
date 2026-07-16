using BookStore.Api.DTOs;
using BookStore.Api.Models;
using BookStore.Api.Repositories;
using BookStore.Api.Rules;

namespace BookStore.Api.Services;

public class BookService
{
    private readonly IRepository<Book> _repository;
    private readonly IEnumerable<IBookDeletionRule> _deletionRules;

    public BookService(
        IRepository<Book> repository,
        IEnumerable<IBookDeletionRule> deletionRules)
    {
        _repository = repository;
        _deletionRules = deletionRules;
    }

    public IEnumerable<BookResponse> GetAll()
    {
        return _repository
            .GetAll()
            .Select(ToResponse);
    }

    public BookResponse? GetById(int id)
    {
        var book = _repository.GetById(id);

        return book is null
            ? null
            : ToResponse(book);
    }

    public BookResponse Create(CreateBookRequest request)
    {
        var book = ToEntity(request);

        _repository.Add(book);

        return ToResponse(book);
    }

    public bool Update(int id, UpdateBookRequest request)
    {
        var book = _repository.GetById(id);

        if (book is null)
            return false;

        book.Title = request.Title;
        book.AuthorId = request.AuthorId;
        book.ISBN = request.ISBN;
        book.PublishedDate = request.PublishedDate;
        book.Price = request.Price;
        book.Stock = request.Stock;
        book.IsPremium = request.IsPremium;
        book.IsLoaned = request.IsLoaned;
        book.IsHistorical = request.IsHistorical;
        book.IsArchived = request.IsArchived;

        _repository.Update(book);

        return true;
    }

    public bool Delete(int id)
    {
        var book = _repository.GetById(id);

        if (book is null)
            return false;

        foreach (var rule in _deletionRules)
        {
            rule.Validate(book);
        }

        _repository.Delete(id);

        return true;
    }

    private static Book ToEntity(CreateBookRequest request)
    {
        return new Book
        {
            Title = request.Title,
            AuthorId = request.AuthorId,
            ISBN = request.ISBN,
            PublishedDate = request.PublishedDate,
            Price = request.Price,
            Stock = request.Stock,
            IsPremium = request.IsPremium,
            IsLoaned = request.IsLoaned,
            IsHistorical = request.IsHistorical,
            IsArchived = request.IsArchived
        };
    }

    private static BookResponse ToResponse(Book book)
    {
        return new BookResponse
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author.Name,
            ISBN = book.ISBN,
            PublishedDate = book.PublishedDate,
            Price = book.Price,
            Stock = book.Stock,
            IsPremium = book.IsPremium,
            IsLoaned = book.IsLoaned,
            IsHistorical = book.IsHistorical,
            IsArchived = book.IsArchived
        };
    }
}