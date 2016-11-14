using System;

namespace Analyzer.StateMachine
{
	internal class StateMachineException : Exception
	{
		public StateMachineException(string message)
			: base(message)
		{

		}
	}
}
