using Analyzer.StateMachine.States;
using Analyzer.Extensions;

namespace Analyzer.StateMachine.Rules
{
	internal sealed class OperandRule : ITransitionRule
	{
		public IState SourceState { get; set; }
		public IState DestState { get; set; }
		public int Priority { get; set; }

		public OperandRule(IState sourceState, IState destState, int priority)
		{
			SourceState = sourceState;
			DestState = destState;
			Priority = priority;
		}

		public bool CanTransit(string param)
		{
			FilterOperator? castedOperator = param.ToFilterOperator();
			if (castedOperator == null)
			{
				return !string.IsNullOrWhiteSpace(param);
			}

			return false;
		}
	}
}
