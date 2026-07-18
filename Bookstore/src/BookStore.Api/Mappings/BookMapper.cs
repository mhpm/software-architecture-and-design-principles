using BookStore.Api.DTOs;
using BookStore.Api.Models;

namespace BookStore.Api.Mappings;

public static class BookMapper
{
    public static Book ToEntity(this CreateBookRequest request)
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

    public static BookResponse ToResponse(this Book book)
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

    public static void UpdateFrom(this Book book, UpdateBookRequest request)
    {
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
    }
}