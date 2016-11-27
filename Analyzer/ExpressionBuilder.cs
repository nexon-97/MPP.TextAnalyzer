using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;

namespace Analyzer
{
	using FilterDelegate = Func<string[], bool>;
	using LeafDelegate = Func<string, bool>;

	public class ExpressionBuilder
	{
		public ILogger Logger { get; set; }

		public Expression<FilterDelegate> Build(string expression)
		{
			if (expression == null)
			{
				throw new ArgumentNullException(nameof(expression));
			}

			QueryParser parser = new QueryParser();
			AnalyzerElement[] polishNotation = parser.Parse(expression);

			if (polishNotation != null)
			{
				return BuildExpression(new Stack<AnalyzerElement>(polishNotation));
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

			InvocationExpression invokeLeft = null;
			InvocationExpression invokeRight = null;

			if (left != null) invokeLeft = Expression.Invoke(left, new[] { param });
			if (right != null) invokeRight = Expression.Invoke(right, new[] { param });

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

		private Expression<FilterDelegate> BuildExpression(Stack<AnalyzerElement> stack)
		{
			if (stack.Count > 0)
			{
				var element = stack.Pop();

				if (element.IsOperator)
				{
					if (element.Operator == FilterOperator.Not)
					{
						var operandExpression = BuildExpression(stack);
						return JoinExpressions(null, operandExpression, element.Operator);
					}
					else
					{
						var leftOperand = BuildExpression(stack);
						var rightOperand = BuildExpression(stack);

						return JoinExpressions(leftOperand, rightOperand, element.Operator);
					}
				}
				else
				{
					var leaf = BuildEqualityCheck(element.Data);
					return BuildContainerCheck(leaf);
				}
			}
		
			return null;
		}
	}
}
