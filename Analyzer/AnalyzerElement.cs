namespace Analyzer
{
	internal class AnalyzerElement
	{
		public AnalyzerElement Left { get; set; }
		public AnalyzerElement Right { get; set; }
		public FilterOperator Operator { get; set; }
		public string Data { get; set; }
	}
}
