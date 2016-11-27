namespace Analyzer.StateMachine.States
{
	internal class BraceCloseState : IState
	{
		public void OnStateEnter(IState from, string item)
		{
			if (StateMachine.BracketFactor <= 0)
			{
				throw new StateMachineException("Invalid brackets.");
			}
			else
			{
				StateMachine.BracketFactor--;
			}
		}
	}
}
