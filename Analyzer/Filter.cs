using System;

namespace Analyzer
{
	using FilterDelegate = Func<string[], bool>;

	public sealed class Filter : IFilter
	{
		// скомпилированное в делегат дерево выражений
		private readonly FilterDelegate filter;

		public Filter(FilterDelegate filter)
		{
			this.filter = filter;
		}

		public bool Verify(string[] words)
		{
			if (words == null)
			{
				throw new ArgumentNullException(nameof(words));
			}

			return filter.Invoke(words);
		}
	}
}
