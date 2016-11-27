using Analyzer.StateMachine.States;

namespace Analyzer.StateMachine.Rules
{
	internal sealed class FinishRule : ITransitionRule
	{
		public IState SourceState { get; set; }
		public IState DestState { get; set; }
		public int Priority { get; set; }

		public FinishRule(IState sourceState, IState destState, int priority)
		{
			SourceState = sourceState;
			DestState = destState;
			Priority = priority;
		}

		public bool CanTransit(string param)
		{
			return string.IsNullOrWhiteSpace(param);
		}
	}
}
