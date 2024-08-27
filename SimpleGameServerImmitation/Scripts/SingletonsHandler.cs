namespace SimpleGameServerImmitation.Scripts
{
	// В идеале нужно использовать Dependency Injection или расмотреть другие подходы. Но что бы не тратить на это время, тут реализовано через своего рода singleton
	public static class SingletonsHandler
	{
		public static Database database;
		public static Client client;
		public static CurrentSessionHandler currentSession;
	}
}