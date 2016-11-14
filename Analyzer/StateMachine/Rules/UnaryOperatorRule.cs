﻿using Analyzer.Extensions;
using Analyzer.StateMachine.States;

namespace Analyzer.StateMachine.Rules
{
	internal class UnaryOperatorRule : ITransitionRule
	{
		public IState SourceState { get; set; }
		public IState DestState { get; set; }
		public int Priority { get; set; }

		public UnaryOperatorRule(IState sourceState, IState destState, int priority)
		{
			SourceState = sourceState;
			DestState = destState;
			Priority = priority;
		}

		public bool CanTransit(string param)
		{
			FilterOperator? castedOperator = param.ToFilterOperator();
			if (castedOperator != null)
			{
				return (castedOperator == FilterOperator.Not);
			}

			return false;
		}
	}
}
