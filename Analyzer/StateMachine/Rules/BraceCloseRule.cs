using Analyzer.StateMachine.States;

namespace Analyzer.StateMachine.Rules
{
	internal sealed class BraceCloseRule : ITransitionRule
	{
		private const string BraceCloser = ")";

		public IState SourceState { get; set; }
		public IState DestState { get; set; }
		public int Priority { get; set; }

		public BraceCloseRule(IState sourceState, IState destState, int priority)
		{
			SourceState = sourceState;
			DestState = destState;
			Priority = priority;
		}

		public bool CanTransit(string param)
		{
			return param.Equals(BraceCloser);
		}
	}
}
