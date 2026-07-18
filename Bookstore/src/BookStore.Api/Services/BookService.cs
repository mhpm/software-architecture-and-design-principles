using BookStore.Api.DTOs;
using BookStore.Api.Mappings;
using BookStore.Api.Repositories;
using BookStore.Api.Rules;

namespace BookStore.Api.Services;

public class BookService(IBookRepository repository, IEnumerable<IBookDeletionRule> deletionRules)
{
    public IEnumerable<BookResponse> GetAll()
    {
        return repository
            .GetAll()
            .Select(book => book.ToResponse());
    }

    public BookResponse? GetById(int id)
    {
        var book = repository.GetById(id);

        return book?.ToResponse();
    }

    public BookResponse Create(CreateBookRequest request)
    {
        var book = request.ToEntity();

        repository.Add(book);

        return book.ToResponse();
    }

    public bool Update(int id, UpdateBookRequest request)
    {
        var book = repository.GetById(id);

        if (book is null)
            return false;

        book.UpdateFrom(request);

        repository.Update(book);

        return true;
    }

    public bool Delete(int id)
    {
        var book = repository.GetById(id);

        if (book is null)
            return false;

        foreach (var rule in deletionRules)
        {
            rule.Validate(book);
        }

        repository.Delete(id);

        return true;
    }

}