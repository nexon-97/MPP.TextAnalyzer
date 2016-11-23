using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer;

namespace UnitTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void TestMethod1()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			builder.Build(null);
		}
	}
}
