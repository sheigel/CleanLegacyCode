using System;
using System.Linq.Expressions;

namespace LegacyCode.Tests
{
    internal static class PrivateSetter
    {
        public static void SetProperty<TSource, TProperty>(TSource source, Expression<Func<TSource, TProperty>> expression,
            TProperty authors)
        {
            source.GetType().GetProperty(GetPropertyName(expression)).SetValue(source, authors, null);
        }

        private static string GetPropertyName<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyIdentifier)
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