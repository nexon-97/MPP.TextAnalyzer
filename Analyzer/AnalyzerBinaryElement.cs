namespace Analyzer
{
	internal class AnalyzerBinaryElement : AnalyzerElement
	{
		public AnalyzerElement Left { get; set; }
		public AnalyzerElement Right { get; set; }
		public FilterOperator Operator { get; set; }
	}
}
