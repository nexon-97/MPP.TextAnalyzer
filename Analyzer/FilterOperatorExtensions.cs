using System;
using System.Linq.Expressions;

namespace Analyzer.Extensions
{
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

		public static ExpressionType GetExpressionDelegate(this FilterOperator op)
		{
			switch (op)
			{
				case FilterOperator.And:
					return ExpressionType.AndAlso;
				case FilterOperator.Or:
					return ExpressionType.OrElse;
				case FilterOperator.Not:
					return ExpressionType.NotEqual;
			}

			return ExpressionType.Constant;
		}
	}
}
