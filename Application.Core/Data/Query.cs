using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Application.Core.Data
{
    public class Query<TModel> : IQuery<TModel>
    {
        private readonly IList<string> _includedPropertiesPath = new List<string>();
        private readonly IList<Expression<Func<TModel, bool>>> _filters = new List<Expression<Func<TModel, bool>>>();

        public Query()
        {
        }

        public Query(params Expression<Func<TModel, bool>>[] filters)
            : this()
        {
	        foreach (var expression in filters)
	        {
				_filters.Add(expression);
	        }
        }

        public IList<Expression<Func<TModel, bool>>> Filters
        {
            get { return _filters; }
        }

        public IList<string> IncludedProperties
        {
            get
            {
                return _includedPropertiesPath;
            }
        }

        public virtual IQuery<TModel> AddFilterClause(Expression<Func<TModel, bool>> filterClause)
        {
            _filters.Add(filterClause);

            return this;
        }

        public IQuery<TModel> Include<TResult>(Expression<Func<TModel, TResult>> includeExpression)
        {
            string result;

            if (TryParsePath(includeExpression.Body, out result))
            {
                _includedPropertiesPath.Add(result);
            }

            return this;
        }

        protected static bool TryParsePath(Expression expression, out string path)
        {
            path = null;
            Expression expression2 = RemoveConvert(expression);
            var memberExpression = expression2 as MemberExpression;
            var methodCallExpression = expression2 as MethodCallExpression;
            if (memberExpression != null)
            {
                string name = memberExpression.Member.Name;
                string text;
                if (!TryParsePath(memberExpression.Expression, out text))
                {
                    return false;
                }

                path = (text == null) ? name : (text + "." + name);
            }
            else
            {
                if (methodCallExpression != null)
                {
                    if (methodCallExpression.Method.Name == "Select" && methodCallExpression.Arguments.Count == 2)
                    {
                        string text2;
                        if (!TryParsePath(methodCallExpression.Arguments[0], out text2))
                        {
                            return false;
                        }

                        if (text2 != null)
                        {
                            var lambdaExpression = methodCallExpression.Arguments[1] as LambdaExpression;
                            if (lambdaExpression != null)
                            {
                                string text3;
                                if (!TryParsePath(lambdaExpression.Body, out text3))
                                {
                                    return false;
                                }

                                if (text3 != null)
                                {
                                    path = text2 + "." + text3;
                                    return true;
                                }
                            }
                        }
                    }

                    return false;
                }
            }

            return true;
        }

        protected static Expression RemoveConvert(Expression expression)
        {
            while (expression != null && (expression.NodeType == ExpressionType.Convert || expression.NodeType == ExpressionType.ConvertChecked))
            {
                expression = RemoveConvert(((UnaryExpression)expression).Operand);
            }

            return expression;
        }
    }
}
