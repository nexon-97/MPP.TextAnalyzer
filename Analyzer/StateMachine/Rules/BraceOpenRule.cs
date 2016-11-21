using Analyzer.StateMachine.States;

namespace Analyzer.StateMachine.Rules
{
	internal sealed class BraceOpenRule : ITransitionRule
	{
		private const string BraceOpener = "(";

		public IState SourceState { get; set; }
		public IState DestState { get; set; }
		public int Priority { get; set; }

		public BraceOpenRule(IState sourceState, IState destState, int priority)
		{
			SourceState = sourceState;
			DestState = destState;
			Priority = priority;
		}

		public bool CanTransit(string param)
		{
			return param.Equals(BraceOpener);
		}
	}
}
