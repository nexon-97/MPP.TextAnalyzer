using System;
using Analyzer;

namespace UnitTests
{
	internal class Program
	{
		static void Main(string[] args)
		{
			ExpressionBuilder builder = new ExpressionBuilder();

			var filterExpression = builder.Build("not (thor and loki) or now");
			if (filterExpression != null)
			{
				Console.WriteLine("Generated expression:\n" + filterExpression.ToString());

				Filter testFilter = new Filter(filterExpression.Compile());

				string[] input =
				{
					"thor", "fucks", "loki", "hard", "right", "now"
				};
				bool valid = testFilter.Verify(input);

				Console.WriteLine("Valid: " + valid.ToString());
			}

			Console.ReadLine();
		}
	}
}
