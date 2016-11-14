using System;
using System.Linq;

namespace Analyzer
{
	internal class QueryParser
	{
		private static readonly char[] querySeparators =
		{
			'\t',
			' '
		};

		public string[] Parse(string query)
		{
			if (query == null)
			{
				throw new ArgumentNullException(nameof(query));
			}

			string[] queryParts = query.Split(querySeparators);
			queryParts = queryParts.Where(x => !string.IsNullOrEmpty(x)).ToArray();

			return queryParts;
		}
	}
}
