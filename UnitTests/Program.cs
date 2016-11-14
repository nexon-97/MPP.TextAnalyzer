using System;
using Analyzer;
using System.Linq.Expressions;

namespace UnitTests
{
	using FilterDelegate = Func<string[], bool>;
	using LeafDelegate = Func<string, bool>;

	internal class Program
	{
		static void Main(string[] args)
		{
			ExpressionBuilder builder = new ExpressionBuilder();

			builder.Build("thor and loki not");

			/*Expression<FilterDelegate> filter = builder.Build("thor and loki");
			FilterDelegate compiledFilter = filter.Compile();

			string[] text =
			{
				"thor", "loki", "loki", "hard"
			};
			bool contains = compiledFilter(text);

			Console.WriteLine(contains.ToString());*/

			Console.ReadLine();
		}
	}
}
