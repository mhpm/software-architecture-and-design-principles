using BookStore.Api.Models;
using BookStore.Api.Rules;
using BookStore.Api.Repositories;

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

    public IEnumerable<Book> GetAll()
    {
        return _repository.GetAll();
    }

    public Book? GetById(int id)
    {
        return _repository.GetById(id);
    }

    public Book Create(Book book)
    {
        _repository.Add(book);

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

        _repository.Update(book);

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

        _repository.Delete(id);

        return true;
    }
}