using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Analyzer;

namespace UnitTests
{
	[TestClass]
	public class ExpressionBuilderExceptionsTest
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CheckNullArgument()
		{
			ExpressionBuilder builder = new ExpressionBuilder();
			builder.Build(null);
		}
	}
}
