namespace Analyzer
{
	internal class AnalyzerUnaryElement : AnalyzerElement
	{
		public FilterOperator Operator { get; set; }
		public AnalyzerElement Operand { get; set; }
	}
}
