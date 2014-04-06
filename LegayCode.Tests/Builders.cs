using System;
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