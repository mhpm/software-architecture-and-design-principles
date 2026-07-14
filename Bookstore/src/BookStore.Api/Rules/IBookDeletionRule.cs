using BookStore.Api.Models;

namespace BookStore.Api.Rules;

public interface IBookDeletionRule
{
    void Validate(Book book);
}