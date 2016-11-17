using System;
using System.Linq.Expressions;

namespace Analyzer
{
	using FilterDelegate = Func<string[], bool>;

	internal class AnalyzerElement
	{
		public string Data { get; set; }

		public virtual Expression<FilterDelegate> ToExpression()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			var equalityCheck = builder.BuildEqualityCheck(Data);
			return builder.BuildContainerCheck(equalityCheck);
		}
	}
}
