using SimpleGameServerImmitation.Scripts.Databases;

// Простой класс, симулирующий уже существующую базу данных с какими то данными
namespace SimpleGameServerImmitation.Scripts.Mocks
{
	public class MockDatabase : Database
	{
		public MockDatabase() 
		{ 
			usersDatabase = new MockUsersDatabase();
			serversDatabase = new MockServersDatabase();
		}

		private class MockUsersDatabase : UsersDatabase
		{
			public MockUsersDatabase()
			{
				CreateUser("Test", "TestPass");
				CreateUser("User1", "12345678");
				CreateUser("User2", "12345678");
				CreateUser("User3", "12345678");
				CreateUser("Admin", "Admin");
			}
		}

		private class MockServersDatabase : ServersDatabase
		{
			public MockServersDatabase()
			{
				data.Add(new ServerInfo() { id = 0, name = "PotatoGameServer", map = new Map(56, 56), players = new List<Player>{
					new Player(){userInfoId = 1, currentHp = 100, maxHp = 200, damage = 10, money = 50, posX = 10, posY = 17},
					new Player(){userInfoId = 2, currentHp = 120, maxHp = 120, damage = 20, money = 74, posX = 18, posY = 50},
					new Player(){userInfoId = 3, currentHp = 55, maxHp = 225, damage = 30, money = 1000, posX = 33, posY = 33},
				}});
				data.Add(new ServerInfo() { id = 1, name = "TestServer", map = new Map(128, 128),
					players = new List<Player>{
					new Player(){userInfoId = 1, currentHp = 100, maxHp = 200, damage = 10, money = 50, posX = 10, posY = 17},
					new Player(){userInfoId = 2, currentHp = 120, maxHp = 120, damage = 20, money = 74, posX = 18, posY = 50},
					new Player(){userInfoId = 3, currentHp = 55, maxHp = 225, damage = 30, money = 1000, posX = 33, posY = 33},
				}
				});
				data.Add(new ServerInfo() { id = 2, name = "BestServer", map = new Map(32, 32),
					players = new List<Player>{
					new Player(){userInfoId = 1, currentHp = 100, maxHp = 200, damage = 10, money = 50, posX = 10, posY = 14},
					new Player(){userInfoId = 2, currentHp = 120, maxHp = 120, damage = 20, money = 74, posX = 18, posY = 25},
					new Player(){userInfoId = 3, currentHp = 55, maxHp = 225, damage = 30, money = 1000, posX = 17, posY = 17},
				}
				});
			}
		}
	}
}