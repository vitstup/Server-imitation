namespace SimpleGameServerImmitation.Scripts.Databases
{
	public class ServersDatabase : TypedDatabase<ServerInfo>
	{
		public bool HaveSuchServer(int id)
		{
			return data.Any(server => server.id == id);
		}

		// Этот метод обновляет все данные сервера, посколько у меня могут меняться только игроки то я передаю только их 
		public void UpdateServerGameInfoById(int id, List<Player> players)
		{
			if (!HaveSuchServer(id)) return;

			data.FirstOrDefault(server => server.id == id).players = players;
		}
	}

	public class ServerInfo
	{
		public int id;
		public string name;

		public Map map;
		public List<Player> players;
	}
}