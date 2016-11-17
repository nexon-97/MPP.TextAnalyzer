using System.Linq.Expressions;
using System;

namespace Analyzer
{
	using FilterDelegate = Func<string[], bool>;

	internal class AnalyzerBinaryElement : AnalyzerElement
	{
		public AnalyzerElement Left { get; set; }
		public AnalyzerElement Right { get; set; }
		public FilterOperator Operator { get; set; }

		public override Expression<FilterDelegate> ToExpression()
		{
			Expression<FilterDelegate> leftExpr = Left.ToExpression();
			Expression<FilterDelegate> rightExpr = Right.ToExpression();

			ExpressionBuilder builder = new ExpressionBuilder();
			return builder.JoinExpressions(leftExpr, rightExpr, Operator);
		}
	}
}
