namespace Analyzer.StateMachine.States
{
	internal interface IState
	{
		void OnStateEnter(IState from);
	}
}
