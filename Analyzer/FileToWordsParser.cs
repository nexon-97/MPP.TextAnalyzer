using System;
using System.Collections.Generic;
using System.IO;

namespace WpfClient.Model
{
	public class FileToWordsParser
	{
		private char[] delimeters;

		public FileToWordsParser()
		{
			BuildDelimetersList();
		}

		private void BuildDelimetersList()
		{
			List<char> delimetersList = new List<char>();

			for (char c = (char)1; c < (char)127; c++)
			{
				if (!char.IsLetterOrDigit(c))
				{
					delimetersList.Add(c);
				}
			}

			delimeters = delimetersList.ToArray();
		}

		public string[] Parse(string path)
		{
			HashSet<string> words = new HashSet<string>();

			string fileText = null;
			try
			{
				fileText = File.ReadAllText(path);
				return fileText.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
			}
			catch (Exception)
			{
				return null;
			}
		}
	}
}
