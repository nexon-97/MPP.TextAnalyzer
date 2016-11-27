using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer;

namespace UnitTests
{
	[TestClass]
	public class QueryParserTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void NullArgumentTest()
		{
			QueryParser parser = new QueryParser();
			var result = parser.Parse(null);
		}

		[TestMethod]
		public void ValidQueryTest1()
		{
			QueryParser parser = new QueryParser();
			var result = parser.Parse("thor and loki");

			Assert.IsInstanceOfType(result, typeof(AnalyzerElement[]));
		}

		[TestMethod]
		public void ValidQueryTest2()
		{
			QueryParser parser = new QueryParser();
			var result = parser.Parse("(foo and bar) and not mama");

			Assert.IsInstanceOfType(result, typeof(AnalyzerElement[]));
		}

		[TestMethod]
		public void ValidQueryTest3()
		{
			QueryParser parser = new QueryParser();
			var result = parser.Parse("((a and b) or (c and d)) or e");

			Assert.IsInstanceOfType(result, typeof(AnalyzerElement[]));
		}

		[TestMethod]
		public void InvalidQueryTest1()
		{
			QueryParser parser = new QueryParser();
			var result = parser.Parse("(someword and )");

			Assert.IsNull(result);
		}

		[TestMethod]
		public void InvalidQueryTest2()
		{
			QueryParser parser = new QueryParser();
			var result = parser.Parse("hello)");

			Assert.IsNull(result);
		}

		[TestMethod]
		public void InvalidQueryTest3()
		{
			QueryParser parser = new QueryParser();
			var result = parser.Parse("not and word");

			Assert.IsNull(result);
		}
	}
}
