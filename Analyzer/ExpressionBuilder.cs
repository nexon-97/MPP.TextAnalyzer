using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Analyzer
{
	using FilterDelegate = Func<string[], bool>;
	using LeafDelegate = Func<string, bool>;

	public class ExpressionBuilder
	{
		public ILogger Logger { get; set; }
		public Expression<Func<string[], string, bool>> OccurenceCondition { get; private set; }

		public ExpressionBuilder()
		{
			OccurenceCondition = ((text, item) => Array.Exists(text, o => o.Equals(item)));
		}

		public Expression<FilterDelegate> Build(string expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException(nameof(expression));
			}

			QueryParser parser = new QueryParser();
			AnalyzerElement rootComponent = parser.Parse(expression);

			if (rootComponent != null)
			{
				return rootComponent.ToExpression();
			}

			return null;
		}

		private void LogMessage(string message)
		{
			if (Logger != null)
			{
				Logger.LogMessage(message);
			}
		}

		public Expression<FilterDelegate> BuildContainerCheck(Expression<LeafDelegate> equalityCheck)
		{
			ParameterExpression container = Expression.Parameter(typeof(string[]), "container");
			Predicate<string> predicate = new Predicate<string>(equalityCheck.Compile());
			ConstantExpression predicateConst = Expression.Constant(predicate, typeof(Predicate<string>));

			MethodInfo existsMethod = typeof(Array).GetMethod("Exists").MakeGenericMethod(typeof(string));
			MethodCallExpression existsCall = Expression.Call(null, existsMethod, container, predicateConst);

			return Expression.Lambda<FilterDelegate>(existsCall, new[] { container });
		}

		public Expression<LeafDelegate> BuildEqualityCheck(string value)
		{
			ParameterExpression pe = Expression.Parameter(typeof(string), "o");
			ConstantExpression constant = Expression.Constant(value, typeof(string));

			MethodInfo equalsMethod = typeof(string).GetMethod("Equals", new[] { typeof(string) });
			MethodCallExpression equalsCall = Expression.Call(pe, equalsMethod, constant);

			return Expression.Lambda<LeafDelegate>(equalsCall, new[] { pe });
		}

		public Expression<FilterDelegate> JoinExpressions(LambdaExpression left, LambdaExpression right, FilterOperator op)
		{
			ParameterExpression param = Expression.Parameter(typeof(string[]), "container");

			var invokeLeft = Expression.Invoke(left, new[] { param });
			var invokeRight = Expression.Invoke(right, new[] { param });

			Expression body = null;
			switch (op)
			{
				case FilterOperator.And:
					body = Expression.AndAlso(invokeLeft, invokeRight);
					break;
				case FilterOperator.Or:
					body = Expression.OrElse(invokeLeft, invokeRight);
					break;
				case FilterOperator.Not:
					body = Expression.Not(invokeRight);
					break;
			}

			return Expression.Lambda<FilterDelegate>(body, new[] { param });
		}
	}
}
