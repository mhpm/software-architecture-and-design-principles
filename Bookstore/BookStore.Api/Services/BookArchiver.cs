using BookStore.Api.Models;

namespace BookStore.Api.Services;

public class BookArchiver
{
    public void Archive(Book book)
    {
        if (book.IsArchived)
        {
            Console.WriteLine($"El libro {book.Title} ya se encuentra archivado.");

            return;
        }

        book.IsArchived = true;

        Console.WriteLine($"Libro {book.Title} archivado exitosamente.");
    }
}