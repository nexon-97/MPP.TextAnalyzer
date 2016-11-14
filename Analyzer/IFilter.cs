namespace Analyzer
{
	public interface IFilter
	{
		bool Verify(string[] words);
	}
}
