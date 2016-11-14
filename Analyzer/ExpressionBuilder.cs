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
			string[] components = parser.Parse(expression);


			return null;
		}

		private Expression ComposeNextPair(Expression current, string[] text, string[] elements)
		{
			if (elements.Length < 2)
			{
				throw new ArgumentException("Unable to add condition.");
			}

			bool hasLValue = (current != null);

			// Get next operator
			int operatorPosition = hasLValue ? 0 : 1;
			FilterOperator? o = elements[operatorPosition].ToFilterOperator();
			if (o == null)
			{
				return null;
			}

			// Get operator expression
			var operatorExpression = o.Value.GetExpressionDelegate();

			if (hasLValue)
			{
				//return OccurenceCondition;
				//return operatorExpression(current, );
			}
			else
			{
				return operatorExpression(OccurenceCondition, OccurenceCondition);

				//Expression.Constant(elements[0]);

				
				//Expression.Constant(text);
				//Expression.Constant(elements[2]);
			}

			int rValuePosition = hasLValue ? 1 : 2;
			string rValue = elements[rValuePosition];




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

		public Expression<LeafDelegate> JoinExpressions(LambdaExpression left, LambdaExpression right, FilterOperator op)
		{
			ParameterExpression param = Expression.Parameter(typeof(string), "x");

			var invokeLeft = Expression.Invoke(left, new[] { param });
			var invokeRight = Expression.Invoke(right, new[] { param });

			BinaryExpression body = null;
			switch (op)
			{
				case FilterOperator.And:
					body = Expression.AndAlso(invokeLeft, invokeRight);
					break;
				case FilterOperator.Or:
					body = Expression.OrElse(invokeLeft, invokeRight);
					break;
				case FilterOperator.Not:
					body = Expression.NotEqual(invokeLeft, invokeRight);
					break;
			}

			return Expression.Lambda<LeafDelegate>(body, new[] { param });
		}
	}
}
