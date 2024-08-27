using SimpleGameServerImmitation.Scripts.Interfaces;

namespace SimpleGameServerImmitation.Scripts.States
{
	public abstract class ConsoleShowerState : BasicLogicState, IConsoleInterfaceShower
	{
		protected ConsoleShowerState(BasicStateMachine stateMachine) : base(stateMachine)
		{

		}

		public void ShowInterface()
		{
			StateEntered();
		}
	}
}