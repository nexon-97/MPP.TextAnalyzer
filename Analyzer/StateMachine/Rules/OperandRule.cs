using Analyzer.StateMachine.States;

namespace Analyzer.StateMachine.Rules
{
	internal class OperandRule : ITransitionRule
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
			return !string.IsNullOrWhiteSpace(param);
		}
	}
}
