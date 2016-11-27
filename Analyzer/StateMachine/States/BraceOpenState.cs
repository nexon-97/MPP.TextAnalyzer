namespace Analyzer.StateMachine.States
{
	internal class BraceOpenState : IState
	{
		public void OnStateEnter(IState from, string item)
		{
			StateMachine.BracketFactor++;
		}
	}
}
