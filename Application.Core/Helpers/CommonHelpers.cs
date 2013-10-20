using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Application.Core.Helpers
{
	public class CommonHelpers
	{
		protected string ExtractPropertyName<T>(Expression<Func<T>> propertyExpression)
		{
			if (propertyExpression == null)
			{
				throw new ArgumentNullException("propertyExpression");
			}

			var memberExpression = propertyExpression.Body as MemberExpression;

			if (memberExpression == null)
			{
				throw new ArgumentException();
			}

			var propertyInfo = memberExpression.Member as PropertyInfo;

			if (propertyInfo == null)
			{
				throw new ArgumentException();
			}

			var getMethod = propertyInfo.GetGetMethod(true);
			if (getMethod.IsStatic)
			{
				throw new ArgumentException();
			}

			return memberExpression.Member.Name;
		}
	}
}
