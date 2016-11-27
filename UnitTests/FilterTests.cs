using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer;

namespace UnitTests
{
	[TestClass]
	public class FilterTests
	{
		private string[] ParseInput(string query)
		{
			FileToWordsParser inputParser = new FileToWordsParser();
			return inputParser.ParseText(query);
		}

		private Filter BuildFilter(string query)
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			var filterExpression = builder.Build(query);

			return new Filter(filterExpression.Compile());
		}

		[TestMethod]
		public void FilterTest1()
		{
			const string inputText = "mama mila ramu. vasya breaks the rules!";
			const string filterText = "mama and vasya and (ramu and not rama)";

			var input = ParseInput(inputText);
			var filter = BuildFilter(filterText);

			Assert.IsTrue(filter.Verify(input));
		}

		[TestMethod]
		public void FilterTest2()
		{
			const string inputText = "mama mila ramu. vasya breaks the rules!";
			const string filterText = "mama and (foo or bar)";

			var input = ParseInput(inputText);
			var filter = BuildFilter(filterText);

			Assert.IsFalse(filter.Verify(input));
		}

		[TestMethod]
		public void FilterTest3()
		{
			const string inputText = "Folks like Thor and Loki";
			const string filterText = "thor and loki";

			var input = ParseInput(inputText);
			var filter = BuildFilter(filterText);

			Assert.IsTrue(filter.Verify(input));
		}

		[TestMethod]
		public void FilterTest4()
		{
			const string inputText = "Thor and Loki like to fight each other.";
			const string filterText = "(thor and loki) and (like and not love)";

			var input = ParseInput(inputText);
			var filter = BuildFilter(filterText);

			Assert.IsTrue(filter.Verify(input));
		}

		[TestMethod]
		public void FilterTest5()
		{
			const string inputText = "- I have some bad news for you. - Oh, it's ok.";
			const string filterText = "(ok or no) and (good or bad)";

			var input = ParseInput(inputText);
			var filter = BuildFilter(filterText);

			Assert.IsTrue(filter.Verify(input));
		}
	}
}
