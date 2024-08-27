using SimpleGameServerImmitation.Scripts.Databases;

namespace SimpleGameServerImmitation.Scripts
{
	public class CurrentSessionHandler
	{
		public UserInfo currentUserInfo;

		// Тут напрямую ссылка на подключенный сервер, в реальном приложении по идее будет какой нибудь класс коннектор 
		public Server currentServer;
	}
}