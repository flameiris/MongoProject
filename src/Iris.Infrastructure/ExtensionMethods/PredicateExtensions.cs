using System;
using System.Linq;
using System.Linq.Expressions;

namespace Iris.Infrastructure.ExtensionMethods
{
    public static class PredicateExtensions
    {
        public static Expression<Func<T, bool>> True<T>() => f => true;

        public static Expression<Func<T, bool>> False<T>() => f => false;

        public static Expression<Func<T, bool>> Or<T>
            (this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var exp1Params = expression1.Parameters.Cast<Expression>();
            var invokedExpression = Expression.Invoke(expression2, exp1Params);
            var orExp = Expression.Or(expression1.Body, invokedExpression);
            return Expression.Lambda<Func<T, bool>>(orExp, expression1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>
            (this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            var exp1Params = expression1.Parameters.Cast<Expression>();
            var invokedExpression = Expression.Invoke(expression2, exp1Params);
            var andExp = Expression.And(expression1.Body, invokedExpression);
            return Expression.Lambda<Func<T, bool>>(andExp, expression1.Parameters);
        }
    }
}
