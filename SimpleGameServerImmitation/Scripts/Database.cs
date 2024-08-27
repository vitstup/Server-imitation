using SimpleGameServerImmitation.Scripts.Databases;

namespace SimpleGameServerImmitation.Scripts
{
	public class Database
	{
		public UsersDatabase usersDatabase { get; protected set; }
		public ServersDatabase serversDatabase { get; protected set; }
	}
}