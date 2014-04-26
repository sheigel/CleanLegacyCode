using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using LegacyCode.Bll;

namespace LegacyCode.Tests
{
    public static class A
    {
        public static BookBuilder Book { get { return new BookBuilder(); } }

        public static BookCollectionBuilder BookCollection { get { return new BookCollectionBuilder(); } }
    }

    public class BookCollectionBuilder
    {
        private readonly List<Book> books = new List<Book>();

        public BookCollection Build()
        {
            var bookCollection = new BookCollection();
            foreach (var book in books)
            {
                bookCollection.Add(book);
            }
            return bookCollection;
        }

        public BookCollectionBuilder WithBooks(params BookBuilder[] bookBuilders)
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
        private int publisherId = 2;

        public Book Build()
        {
            var book = (Book)FormatterServices.GetUninitializedObject(typeof(Book));
            PrivateSetter.SetProperty(book, b => b.Classification, classification);
            PrivateSetter.SetProperty(book, b => b.PublisherId, publisherId);

            PrivateSetter.SetProperty(book, b => book.Authors, new AuthorCollection());

            return book;
        }

        public BookBuilder WithClassification(Classification classification)
        {
            this.classification = classification;
            return this;
        }

        public BookBuilder WithPublisher(Publisher publisher)
        {
            publisherId = (int)publisher;
            return this;
        }
    }
}