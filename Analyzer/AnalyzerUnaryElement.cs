using System;
using System.Linq.Expressions;

namespace Analyzer
{
	using FilterDelegate = Func<string[], bool>;

	internal class AnalyzerUnaryElement : AnalyzerElement
	{
		public FilterOperator Operator { get; set; }
		public AnalyzerElement Operand { get; set; }

		public override Expression<FilterDelegate> ToExpression()
		{
			Expression<FilterDelegate> operandExpr = Operand.ToExpression();

			ExpressionBuilder builder = new ExpressionBuilder();
			return builder.JoinExpressions(null, operandExpr, Operator);
		}
	}
}
