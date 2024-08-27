using SimpleGameServerImmitation.Scripts.Databases;

namespace SimpleGameServerImmitation.Scripts
{
	// Этот класс симулирует запущенные сервера
	public static class ServersSimulation
	{
		private static List<Server> servers = new List<Server>();

		public static void CreateServers(ServerInfo[] serverInfos)
		{
			for (int i = 0; i < serverInfos.Length; i++)
			{
				servers.Add(new Server(serverInfos[i]));
			}
		}

		public static Server GetServerById(int id)
		{
			return servers.FirstOrDefault(server => server.serverId == id);
		}
	}
}