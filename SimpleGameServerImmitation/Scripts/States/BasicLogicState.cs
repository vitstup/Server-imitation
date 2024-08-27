namespace SimpleGameServerImmitation.Scripts.States
{
	public abstract class BasicLogicState
	{
		protected BasicStateMachine stateMachine;

		public BasicLogicState(BasicStateMachine stateMachine)
		{
			this.stateMachine = stateMachine;
		}

		public void ChangeState(BasicLogicState newState)
		{
			stateMachine.ChangeState(newState);
		}

		public abstract void StateEntered();
	}
}