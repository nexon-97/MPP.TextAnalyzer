namespace Analyzer.StateMachine.States
{
	internal class OperandState : BaseState, IState
	{
		public void OnStateEnter(IState from, string item)
		{
			Element = new AnalyzerElement();
			Element.Data = item;

			var binaryOperatorState = from as BinaryOperatorState;
			if (binaryOperatorState != null)
			{
				var binaryElement = binaryOperatorState.Element as AnalyzerBinaryElement;
				binaryElement.Right = Element;

				Element = binaryElement;
			}

			var unaryOperatorState = from as UnaryOperatorState;
			if (unaryOperatorState != null)
			{
				var unaryElement = unaryOperatorState.Element as AnalyzerUnaryElement;
				unaryElement.Operand = Element;

				Element = unaryOperatorState.Element;
			}
		}
	}
}
