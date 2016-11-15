using Analyzer.Extensions;

namespace Analyzer.StateMachine.States
{
	internal class BinaryOperatorState : BaseState, IState
	{
		public void OnStateEnter(IState from, string item)
		{
			var element = new AnalyzerBinaryElement();
			Element = element;

			element.Data = item;
			element.Operator = item.ToFilterOperator().Value;
			
			var operandState = from as OperandState;
			if (operandState != null)
			{
				element.Left = operandState.Element;
			}
		}
	}
}
