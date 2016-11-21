using System;
using System.Collections.Generic;

namespace Analyzer
{
	public class PolishNotationBuilder
	{
		private struct PriorityNote
		{
			public int Priority { get; set; }
			public string Value { get; set; }
		}

		private List<PriorityNote> priorities;
		private const int DefaultPriority = 4;

		public PolishNotationBuilder()
		{
			InitPriorityTable();
		}

		public string[] ConvertFromInfixNotation(string[] expression)
		{
			List<string> output = new List<string>();

			Stack<string> stack = new Stack<string>();
			foreach (var element in expression)
			{
				if (stack.Count == 0 || element.Equals("("))
				{
					stack.Push(element);
				}
				else if (element.Equals(")"))
				{
					while (stack.Count > 0)
					{
						string poppedElement = stack.Pop();
						if (poppedElement.Equals("("))
						{
							break;
						}
						else
						{
							output.Add(poppedElement);
						}
					}
				}
				else
				{
					int priority = GetPriority(element);

					while (stack.Count > 0 && GetPriority(stack.Peek()) >= priority)
					{
						output.Add(stack.Pop());
					}

					stack.Push(element);
				}
			}

			while (stack.Count > 0)
			{
				output.Add(stack.Pop());
			}

			return output.ToArray();
		}

		private int GetPriority(string element)
		{
			const int PriorityNotFound = -1;

			int elementIndex = priorities.FindIndex(o => o.Value.Equals(element));
			if (elementIndex != PriorityNotFound)
			{
				return priorities[elementIndex].Priority;
			}

			return DefaultPriority;
		}

		private void InitPriorityTable()
		{
			priorities = new List<PriorityNote>();

			priorities.Add(new PriorityNote { Priority = 0, Value = "(" });
			priorities.Add(new PriorityNote { Priority = 1, Value = ")" });
			priorities.Add(new PriorityNote { Priority = 2, Value = "and" });
			priorities.Add(new PriorityNote { Priority = 2, Value = "or" });
			priorities.Add(new PriorityNote { Priority = 3, Value = "not" });
		}
	}
}
