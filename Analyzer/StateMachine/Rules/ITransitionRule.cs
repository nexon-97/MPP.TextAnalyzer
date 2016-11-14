using Analyzer.StateMachine.States;

namespace Analyzer.StateMachine.Rules
{
	internal interface ITransitionRule
	{
		IState SourceState { get; set; }
		IState DestState { get; set; }
		int Priority { get; set; }

		bool CanTransit(string param);
	}
}
