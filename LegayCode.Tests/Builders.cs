using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using LegayCode.Bll;

namespace LegayCode.Tests
{
    public static class A
    {
        public static BookBuilder Book
        {
            get { return new BookBuilder(); }
        }

        public static PublisherBookGroupBuilder PublisherBookGroup
        {
            get { return new PublisherBookGroupBuilder(); }
        }
    }

    public class PublisherBookGroupBuilder
    {
        private readonly BookCollection books = new BookCollection();

        public PublisherBookGroup Build()
        {
            return new PublisherBookGroup(books, 2, "Humanitas");
        }

        public PublisherBookGroupBuilder WithBooks(params BookBuilder[] bookBuilders)
        {
            foreach (Book book in bookBuilders.Select(b => b.Build()))
            {
                books.Add(book);
            }
            return this;
        }
    }

    public class BookBuilder
    {
        private Classification classification = Classification.NonFiction;

        public Book Build()
        {
            var book = (Book) FormatterServices.GetUninitializedObject(typeof (Book));
            PrivateSetter.SetProperty(book, b => b.Classification, classification);

            PrivateSetter.SetProperty(book, b => book.Authors, new AuthorCollection());

            return book;
        }

        public BookBuilder WithClassification(Classification classification)
        {
            this.classification = classification;
            return this;
        }
    }
}