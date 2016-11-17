﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace Analyzer
{
	internal class QueryParser
	{
		private static readonly char[] querySeparators =
		{
			'\t',
			' '
		};

		public AnalyzerElement Parse(string query)
		{
			if (query == null)
			{
				throw new ArgumentNullException(nameof(query));
			}

			string[] queryParts = ConvertToStringArray(query);

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
					break;
				}
			}

			bool queryValid = ValidateFinalState(fsm);
			if (queryValid)
			{
				return (fsm.CurrentState as StateMachine.States.BaseState).Element;
			}

			return null;
		}

		private string[] ConvertToStringArray(string query)
		{
			string[] queryParts = query.Split(querySeparators);
			queryParts = queryParts.Where(x => !string.IsNullOrEmpty(x)).ToArray();

			return queryParts;
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
	}
}
