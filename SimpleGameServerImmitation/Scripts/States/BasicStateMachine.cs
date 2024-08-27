namespace SimpleGameServerImmitation.Scripts.States
{
	public abstract class BasicStateMachine
	{
		public BasicLogicState currentLogicState { get; private set; }

		public void ChangeState(BasicLogicState state)
		{
			currentLogicState = state;
			currentLogicState.StateEntered();
		}
	}
}