using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Application.Core.UI.HtmlHelpers
{
	public static class CommonExtensions
	{
		#region CommonExtensions Members

		public static MvcHtmlString DescriptionFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, bool isbuttonToolTip = false)
		{
			var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

			var id = TagBuilder.CreateSanitizedId(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression)));

			if (isbuttonToolTip)
			{
                return MvcHtmlString.Create(string.Format(@"<a href=""#"" class=""tooltip-button"" style=""color:#333;font-size: 24px;"" data-toggle=""tooltip"" data-original-title=""{0}""><span class=""glyphicon glyphicon-exclamation-sign""></span></a>", metadata.Description));
			}

			return MvcHtmlString.Create(string.Format(@"<span id=""descriptionFor{0}""  class=""helperMessage"">{1}</span>", id, metadata.Description));
		}

		public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
		{
			return EnumDropDownListFor(htmlHelper, expression, null);
		}

		public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			Type enumType = GetNonNullableModelType(metadata);
			IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

			IEnumerable<SelectListItem> items = from value in values
												select new SelectListItem
												{
													Text = GetEnumDescription(value),
													Value = value.ToString(),
													Selected = value.Equals(metadata.Model)
												};

			if (metadata.IsNullableValueType)
			{
				items = SingleEmptyItem.Concat(items);
			}

			return htmlHelper.DropDownListFor(expression, items.Where(e => e.Text != null), htmlAttributes);
		}

        public static string ActivePage(this HtmlHelper helper, string controller, string action)
        {
            string classValue = string.Empty;

            string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();
            string currentAction = helper.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();

            if (currentController == controller && currentAction == action)
            {
                classValue = "selected";
            }

            return classValue;
        }

		public static string GetEnumDescription<TEnum>(TEnum value)
		{
			var fieldInfo = value.GetType().GetField(value.ToString());

			var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

			return (attributes.Length > 0)
				? attributes[0].Description
				: null;
		}

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }

		#endregion CommonExtensions Members

		#region Fields

		private static readonly SelectListItem[] SingleEmptyItem = { new SelectListItem { Text = string.Empty, Value = string.Empty } };

		#endregion Fields

		#region Private Members

		private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
		{
			Type realModelType = modelMetadata.ModelType;

			Type underlyingType = Nullable.GetUnderlyingType(realModelType);
			if (underlyingType != null)
			{
				realModelType = underlyingType;
			}

			return realModelType;
		}

		#endregion Private Members
	}
}
