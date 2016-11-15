namespace Analyzer.StateMachine.States
{
	internal class FinalState : BaseState, IState
	{
		public void OnStateEnter(IState from, string item)
		{
			Element = (from as BaseState).Element;
		}
	}
}
