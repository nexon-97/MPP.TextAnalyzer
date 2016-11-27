using System;
using System.Collections.Generic;
using System.Text;
using Analyzer.Extensions;

namespace Analyzer
{
	// Parses input string to an array of tokens, or an analyzer element

	public sealed class QueryParser
	{
		private const char BracketOpener = '(';
		private const char BracketCloser = ')';

		// Returns an array of analyzer elements, presented in polish notation
		public AnalyzerElement[] Parse(string query)
		{
			if (query == null)
			{
				throw new ArgumentNullException(nameof(query));
			}

			string[] queryParts = ConvertToTokensArray(query);

			PolishNotationBuilder polishBuilder = new PolishNotationBuilder();
			string[] polishNotation = polishBuilder.ConvertFromInfixNotation(queryParts);

			var fsm = new StateMachine.StateMachine();
			foreach (var item in queryParts)
			{
				try
				{
					fsm.UpdateState(item);
				}
				catch (StateMachine.StateMachineException e)
				{
					Console.WriteLine(e.Message);
					return null;
				}
			}

			bool queryValid = ValidateFinalState(fsm);
			if (queryValid)
			{
				return ToAnalyzerNotation(polishNotation);		
			}

			return null;
		}

		public string[] ConvertToTokensArray(string query)
		{
			List<string> tokens = new List<string>();
			StringBuilder currentTokenBuilder = new StringBuilder();
			string composedToken;

			foreach (char c in query)
			{
				if (IsTokenDelimeter(c))
				{
					composedToken = currentTokenBuilder.ToString();
					if (!string.IsNullOrEmpty(composedToken))
					{
						tokens.Add(composedToken);
					}

					currentTokenBuilder.Clear();

					// Process brackets
					if (c.Equals(BracketOpener) || c.Equals(BracketCloser))
					{
						tokens.Add(new string(c, 1));
					}
				}
				else
				{
					currentTokenBuilder.Append(c);
				}
			}

			// Add last token
			composedToken = currentTokenBuilder.ToString();
			if (!string.IsNullOrEmpty(composedToken))
			{
				tokens.Add(composedToken);
			}

			return tokens.ToArray();
		}

		private bool IsTokenDelimeter(char c)
		{
			return (char.IsWhiteSpace(c) || c.Equals(BracketOpener) || c.Equals(BracketCloser));
		}

		private bool ValidateFinalState(StateMachine.StateMachine fsm)
		{
			try
			{
				fsm.UpdateState(string.Empty);
				Console.WriteLine("Query valid.");

				return true;
			}
			catch (StateMachine.StateMachineException e)
			{
				Console.WriteLine(e.Message);
				Console.WriteLine("Query invalid!");
			}

			return false;
		}

		private AnalyzerElement[] ToAnalyzerNotation(string[] notation)
		{
			List<AnalyzerElement> analyzerNotation = new List<AnalyzerElement>();

			foreach (var element in notation)
			{
				FilterOperator? operatorCast = element.ToFilterOperator();
				if (operatorCast != null)
				{
					analyzerNotation.Add(new AnalyzerElement { IsOperator = true, Operator = operatorCast.Value });
				}
				else
				{
					analyzerNotation.Add(new AnalyzerElement { IsOperator = false, Data = element });
				}
			}

			return analyzerNotation.ToArray();
		}
	}
}
