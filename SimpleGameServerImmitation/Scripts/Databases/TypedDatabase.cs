namespace SimpleGameServerImmitation.Scripts.Databases
{
	public abstract class TypedDatabase<T> where T : class
	{
		public List<T> data { get; protected set; } = new List<T>();
	}
}