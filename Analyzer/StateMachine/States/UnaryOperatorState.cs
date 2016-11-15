using Analyzer.Extensions;

namespace Analyzer.StateMachine.States
{
	internal class UnaryOperatorState : BaseState, IState
	{
		public void OnStateEnter(IState from, string item)
		{
			var element = new AnalyzerUnaryElement();
			Element = element;

			element.Operator = item.ToFilterOperator().Value;
		}
	}
}
