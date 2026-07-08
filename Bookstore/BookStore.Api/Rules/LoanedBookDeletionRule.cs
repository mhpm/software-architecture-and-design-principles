using BookStore.Api.Models;

namespace BookStore.Api.Rules;

public class LoanedBookDeletionRule : IBookDeletionRule
{
    public void Validate(Book book)
    {
        if (book.IsLoaned)
        {
            throw new InvalidOperationException(
                "El libro está prestado y no puede eliminarse.");
        }
    }
}