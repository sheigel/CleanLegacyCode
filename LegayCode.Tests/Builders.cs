using System;
using System.Collections.Generic;
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
        private BookCollection books= new BookCollection();

        public PublisherBookGroup Build()
        {
            return new PublisherBookGroup(books, 2, "Humanitas");
        }

        public PublisherBookGroupBuilder WithBooks(params BookBuilder[] bookBuilders)
        {
            foreach (var book in bookBuilders.Select(b=>b.Build()))
            {
                books.Add(book);
            }
            return this;
        }
    }

    public class BookBuilder
    {
        public Book Build()
        {
            var book = (Book) FormatterServices.GetUninitializedObject(typeof (Book));
            SetProperty(book, b => book.Authors, new AuthorCollection());

            return book;
        }

        private static void SetProperty<TSource, TProperty>(TSource source,
            Expression<Func<TSource, TProperty>> expression, TProperty authors)
        {
            source.GetType().GetProperty(GetPropertyName(expression)).SetValue(source, authors, null);
        }

        private static string GetPropertyName<TSource, TProperty>(
            Expression<Func<TSource, TProperty>> propertyIdentifier)
        {
            var memberExpression = propertyIdentifier.Body as MemberExpression;

            if (memberExpression == null)
            {
                return null;
            }

            return memberExpression.Member.Name;
        }
    }
}