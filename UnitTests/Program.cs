using System;
using Analyzer;

namespace UnitTests
{
	internal class Program
	{
		static void Main(string[] args)
		{
			string[] input =
			{
				"thor", "fucks", "loki", "hard"
			};	

			ExpressionBuilder builder = new ExpressionBuilder();

			var filterExpression = builder.Build("thor and loki");
			if (filterExpression != null)
			{
				Console.WriteLine("Generated expression:\n" + filterExpression.ToString());

				Filter testFilter = new Filter(filterExpression.Compile());
				bool valid = testFilter.Verify(input);

				Console.WriteLine("Valid: " + valid.ToString());
			}

			Console.ReadLine();
		}
	}
}
