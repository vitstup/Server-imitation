using SimpleGameServerImmitation.Scripts.Databases;

namespace SimpleGameServerImmitation.Scripts
{
	public class Server
	{
		public int serverId { get; private set; }

		// Создание сервера на основе данных из базы данных
		public Server(ServerInfo info)
		{
			serverId = info.id;

			map = info.map;
			players = info.players;
		}

		public Map map { get; private set; }
		public List<Player> players { get; private set; }

		// Простые импементации серверной игровой логики
		// Тут можно понадобавлять всяких проверок на возможность такого действия вообще, читы, существования персонажей и т д
		#region
		public void AddPlayer(Player newPlayer)
		{
			players.Add(newPlayer);
			UpdateDatabase();
		}

		public void DamagePlayer(Player playerToDamage, int damage, Player PlayerThatDamagedHim = null)
		{
			playerToDamage.currentHp -= damage;

			if (playerToDamage.currentHp <= 0) KillPlayer(playerToDamage, PlayerThatDamagedHim);
			else UpdateDatabase();
		}

		private void KillPlayer(Player PlayerToKill, Player PlayerThatKilledHim = null)
		{
			if (PlayerThatKilledHim != null) PlayerThatKilledHim.money += PlayerToKill.money;
			players.Remove(PlayerToKill);
			UpdateDatabase();
		}

		public void RestPlayer(Player playerToRest, int healthToRest = 10)
		{
			playerToRest.currentHp += healthToRest;
			if (playerToRest.currentHp > playerToRest.maxHp) playerToRest.currentHp = playerToRest.maxHp;
			UpdateDatabase();
		}

		public void MovePlayer(Player playerToMove, int newPosX, int newPosY)
		{
			playerToMove.posX = newPosX;
			playerToMove.posY = newPosY;
			UpdateDatabase();
		}
		#endregion

		// Я тут за один раз полностью меняю данные о карте и игроках. В реальном приложении очевидно так делать наверное не стоит, но что бы меньше не тратить тут время реализовал так
		private void UpdateDatabase()
		{
			SingletonsHandler.database.serversDatabase.UpdateServerGameInfoById(serverId, players);
		}
	}
}