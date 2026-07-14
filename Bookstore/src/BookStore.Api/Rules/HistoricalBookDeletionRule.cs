using BookStore.Api.Models;

namespace BookStore.Api.Rules;

public class HistoricalBookDeletionRule : IBookDeletionRule
{
    public void Validate(Book book)
    {
        if (book.IsHistorical && !book.IsArchived)
        {
            throw new InvalidOperationException(
                "Los libros históricos deben archivarse antes de eliminarse.");
        }
    }
}