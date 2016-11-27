using System;
using System.Collections.Generic;
using System.IO;

namespace Analyzer
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
			try
			{
				string fileText = File.ReadAllText(path);
				return ParseText(fileText);
			}
			catch (Exception)
			{
				return null;
			}
		}

		public string[] ParseText(string fileText)
		{
			HashSet<string> words = new HashSet<string>();

			string[] parsedWords = fileText.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);

			// Convert to lowercase
			for (int i = 0; i < parsedWords.Length; i++)
			{
				parsedWords[i] = parsedWords[i].ToLower();
			}

			return parsedWords;
		}
	}
}
