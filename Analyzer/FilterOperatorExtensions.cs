using System;
using System.Linq.Expressions;

namespace Analyzer
{
	using ConditionalOperatorDelegate = Func<Expression, Expression, Expression>;

	internal static class FilterOperatorExtensions
	{
		public static string AsString(this FilterOperator op)
		{
			return op.ToString().ToLower();
		}

		public static FilterOperator? ToFilterOperator(this string text)
		{
			string textLower = text.ToLower();

			var values = Enum.GetValues(typeof(FilterOperator));
			foreach (FilterOperator item in values)
			{
				if (textLower.Equals(item.AsString()))
				{
					return item;
				}
			}

			return null;
		}

		public static ConditionalOperatorDelegate GetExpressionDelegate(this FilterOperator op)
		{
			switch (op)
			{
				case FilterOperator.And:
					return Expression.AndAlso;
				case FilterOperator.Or:
					return Expression.OrElse;
				case FilterOperator.Not:
					return Expression.NotEqual;
				default:
					return null;
			}
		}
	}
}
