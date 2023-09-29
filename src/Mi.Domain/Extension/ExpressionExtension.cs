using System.Linq.Expressions;
using System.Reflection;

using Mi.Domain.Helper;

namespace Mi.Domain.Extension
{
    public static class ExpressionExtension
    {
        // https://stackoverflow.com/questions/457316/combining-two-expressions-expressionfunct-bool/457328#457328

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);
            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
            Expression.OrElse(GuardHelper.NotNull(left), GuardHelper.NotNull(right)), parameter);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
            Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);
            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(GuardHelper.NotNull(left), GuardHelper.NotNull(right)), parameter);
        }

        public static Expression<Func<T, bool>> AndIf<T>(this Expression<Func<T, bool>> expr1, bool condition, Expression<Func<T, bool>> expr2)
        {
            if (!condition)
            {
                return expr1;
            }
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);
            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
            Expression.AndAlso(GuardHelper.NotNull(left), GuardHelper.NotNull(right)), parameter);
        }

        private sealed class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression? Visit(Expression? node)
            {
                if (node == _oldValue)
                    return _newValue;

                return base.Visit(node);
            }
        }

        public static MethodInfo GetMethod<T>(this Expression<T> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!(expression.Body is MethodCallExpression methodCallExpression))
            {
                throw new InvalidCastException("Cannot be converted to MethodCallExpression");
            }
            return methodCallExpression.Method;
        }

        public static MethodCallExpression GetMethodExpression<T>(this Expression<Action<T>> method)
        {
            if (method.Body.NodeType != ExpressionType.Call)
                throw new ArgumentException("Method call expected", method.Body.ToString());
            return (MethodCallExpression)method.Body;
        }

        public static MethodCallExpression GetMethodExpression<T>(this Expression<Func<T, object>> exp)
        {
            switch (exp.Body.NodeType)
            {
                case ExpressionType.Call:
                    return (MethodCallExpression)exp.Body;

                case ExpressionType.Convert:
                    if (exp.Body is UnaryExpression { Operand: MethodCallExpression methodCallExpression })
                    {
                        return methodCallExpression;
                    }
                    break;
            }
            throw new InvalidOperationException($"Method expected: {exp.Body}");
        }

        /// <summary>
        /// GetMemberName
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <typeparam name="TMember">TMember</typeparam>
        /// <param name="memberExpression">get member expression</param>
        /// <returns></returns>
        public static string
            GetMemberName<TEntity, TMember>(this Expression<Func<TEntity, TMember>> memberExpression) =>
            memberExpression.GetMemberInfo().Name;

        /// <summary>
        /// GetMemberInfo
        /// </summary>
        /// <typeparam name="TEntity">TEntity</typeparam>
        /// <typeparam name="TMember">TMember</typeparam>
        /// <param name="expression">get member expression</param>
        /// <returns></returns>
        public static MemberInfo GetMemberInfo<TEntity, TMember>(this Expression<Func<TEntity, TMember>> expression)
        {
            if (expression.NodeType != ExpressionType.Lambda)
            {
                throw new ArgumentException(string.Format("{0} must be lambda expression", nameof(expression)), nameof(expression));
            }

            var lambda = (LambdaExpression)expression;

            var memberExpression = ExtractMemberExpression(lambda.Body);
            if (memberExpression == null)
            {
                throw new ArgumentException(string.Format("{0} must be lambda expression", nameof(expression)), nameof(expression));
            }
            return memberExpression.Member;
        }

        private static MemberExpression ExtractMemberExpression(Expression expression)
        {
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return (MemberExpression)expression;
            }

            if (expression.NodeType == ExpressionType.Convert)
            {
                var operand = ((UnaryExpression)expression).Operand;
                return ExtractMemberExpression(operand);
            }

            throw new InvalidOperationException(nameof(ExtractMemberExpression));
        }
    }

    public class PredicateBuilder
    {
        public static PredicateBuilder Instance => LazyInstance.Value;
        private static Lazy<PredicateBuilder> LazyInstance => new Lazy<PredicateBuilder>(() => new PredicateBuilder());

        public Expression<Func<T, bool>> Create<T>(Expression<Func<T, bool>>? expression = default)
        {
            expression ??= x => true;
            return expression;
        }
    }
}