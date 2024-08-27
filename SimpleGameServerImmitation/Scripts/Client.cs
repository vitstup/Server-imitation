using SimpleGameServerImmitation.Scripts.Interfaces;

namespace SimpleGameServerImmitation.Scripts
{
	// У меня есть прямые обращения из клиента к базе данных и серверу. Хотя понятно в реальном проекте этого бы небыло. 
	// Был бы какой нибудь класс коннектор, который по api получал бы данные и отправлял комманды на сервер. База данных бы взаимодействовала только с сервером.
	// Тут же посколько это будет фактически просто дуюлированием кода и написанием бесмысленных классов с большой тратой времени - это реализованно не было.
	public class Client : IConsoleInterfaceShower
	{
		// Тут напрямую подключение к классу сервера, в реальном приложении такого очевидно быть не может и будет какой нибудь класс коннектор который по api запросам будет передавать/получать данные и комманды между клиентом и сервером
		private Server connectedServer;

		private Player player => connectedServer.players.FirstOrDefault(pl => pl.userInfoId == SingletonsHandler.currentSession.currentUserInfo.id);

		private ClientGameOption[] options;

		public Client()
		{
			options = new ClientGameOption[] {
				new ClientMoveLeftOption(this),
				new ClientMoveRightOption(this),
				new ClientMoveTopOption(this),
				new ClientMoveDownOption(this),
				new ClientRestOption(this),
				new ClientAttackOption(this)
			};
		}

		public void ConnectToServer(int serverId)
		{
			connectedServer = ServersSimulation.GetServerById(serverId);

			if (player == null) CreatePlayer();

			while (player != null)
			{
				ShowInterface();
			}
		}

		private void CreatePlayer()
		{
			Random rand = new Random();
			connectedServer.players.Add(new Player() { 
				userInfoId = SingletonsHandler.currentSession.currentUserInfo.id,
				maxHp = 200,
				currentHp = rand.Next(50, 150),
				posX = rand.Next(0, connectedServer.map.sizeX),
				posY = rand.Next(0, connectedServer.map.sizeY),
				damage = rand.Next(5, 20),
				money = rand.Next(0, 100),
			});
		}

		public void ShowInterface()
		{
			GetServerData();
			DisplayOptionsText();

			var input = Console.ReadLine();

			HandleInput(input);
		}

		private void DisplayOptionsText()
		{
			foreach(var option in options)
			{
				if (option.possibilityCheck()) Console.WriteLine(option.optionTextWhenPossible());
			}
		}

		private void HandleInput(string input)
		{
			bool handledSomeOption = false;

			foreach (var option in options)
			{
				if (option.possibilityCheck() && input == option.optionTextToSelect())
				{
					option.optionSelected();
					handledSomeOption = true;
					break;
				}
			}

			if (!handledSomeOption) Console.WriteLine("К сожалению вы вписали некоректный текст, пожалйста повторите !");
		}

		public void Move(int posX, int posY)
		{
			connectedServer.MovePlayer(player, posX, posY);
		}

		public void Rest()
		{
			connectedServer.RestPlayer(player);
		}

		public void StartBattle()
		{
			var allPlayers = connectedServer.players;

			foreach (var otherPlayer in allPlayers)
			{
				// Игнорируем проверку на самого себя
				if (otherPlayer.userInfoId == player.userInfoId)
				{
					continue;
				}

				// Проверяем, находится ли другой игрок рядом
				bool isNeighbor =
					(otherPlayer.posX == player.posX - 1 && otherPlayer.posY == player.posY) || // Слева
					(otherPlayer.posX == player.posX + 1 && otherPlayer.posY == player.posY) || // Справа
					(otherPlayer.posX == player.posX && otherPlayer.posY == player.posY - 1) || // Сверху
					(otherPlayer.posX == player.posX && otherPlayer.posY == player.posY + 1);   // Снизу

				if (isNeighbor)
				{
					connectedServer.DamagePlayer(otherPlayer, player.damage, player);
					Console.WriteLine($"Игрок: {GetPlayerName(player)} нанёс {player.damage} урона игроку {GetPlayerName(otherPlayer)}");

					if (otherPlayer != null && otherPlayer.currentHp > 0)
					{
						connectedServer.DamagePlayer(player, otherPlayer.damage, otherPlayer);
						Console.WriteLine($"Игрок: {GetPlayerName(otherPlayer)} нанёс {otherPlayer.damage} урона игроку {GetPlayerName(player)}");
					}

					break;
				}
			}
		}
		
		private void GetServerData()
		{
			if (player != null) Console.WriteLine($"Ваш персонаж: {GetPlayerName(player)}");
			else Console.WriteLine("Ваш персонаж мёртв :(");

			foreach(var player in connectedServer.players) DisplayPlayerData(player);
		}

		private void DisplayPlayerData(Player player)
		{
			Console.WriteLine($"{GetPlayerName(player)}, Позиция: {player.posX}: {player.posY}, Hp: {player.currentHp}, Урон: {player.damage}, Монеты: {player.money}");
		}

		private string GetPlayerName(Player player) => SingletonsHandler.database.usersDatabase.GetUserById(player.userInfoId).login;

		private abstract class ClientGameOption
		{
			protected Client client;

			public ClientGameOption(Client client)
			{
				this.client = client;
			}

			public abstract bool possibilityCheck();

			public abstract string optionTextWhenPossible();

			public abstract string optionTextToSelect();

			public abstract void optionSelected(); 
		}

		private class ClientMoveLeftOption : ClientGameOption
		{
			public ClientMoveLeftOption(Client client) : base(client)
			{
			}

			public override void optionSelected()
			{
				client.Move(client.player.posX -1, client.player.posY);
			}

			public override string optionTextToSelect()
			{
				return "Влево";
			}

			public override string optionTextWhenPossible()
			{
				return "Что бы сходить влево - напишите 'Влево'";
			}

			public override bool possibilityCheck()
			{
				return client.player.posX > 0;
			}
		}

		private class ClientMoveRightOption : ClientGameOption
		{
			public ClientMoveRightOption(Client client) : base(client)
			{
			}

			public override void optionSelected()
			{
				client.Move(client.player.posX + 1, client.player.posY);
			}

			public override string optionTextToSelect()
			{
				return "Вправо";
			}

			public override string optionTextWhenPossible()
			{
				return "Что бы сходить вправо - напишите 'Вправо'";
			}

			public override bool possibilityCheck()
			{
				return client.player.posX < client.connectedServer.map.sizeX;
			}
		}

		private class ClientMoveTopOption : ClientGameOption
		{
			public ClientMoveTopOption(Client client) : base(client)
			{
			}

			public override void optionSelected()
			{
				client.Move(client.player.posX, client.player.posY + 1);
			}

			public override string optionTextToSelect()
			{
				return "Вверх";
			}

			public override string optionTextWhenPossible()
			{
				return "Что бы сходить вверх - напишите 'Вверх'";
			}

			public override bool possibilityCheck()
			{
				return client.player.posY < client.connectedServer.map.sizeY;
			}
		}

		private class ClientMoveDownOption : ClientGameOption
		{
			public ClientMoveDownOption(Client client) : base(client)
			{
			}

			public override void optionSelected()
			{
				client.Move(client.player.posX, client.player.posY + 1);
			}

			public override string optionTextToSelect()
			{
				return "Вниз";
			}

			public override string optionTextWhenPossible()
			{
				return "Что бы сходить вниз - напишите 'Вниз'";
			}

			public override bool possibilityCheck()
			{
				return client.player.posY > 0;
			}
		}

		private class ClientRestOption : ClientGameOption
		{
			public ClientRestOption(Client client) : base(client)
			{
			}

			public override void optionSelected()
			{
				client.Rest();
			}

			public override string optionTextToSelect()
			{
				return "Отдых";
			}

			public override string optionTextWhenPossible()
			{
				return "Что бы отдохнуть - напишите 'Отдых'";
			}

			public override bool possibilityCheck()
			{
				return true;
			}
		}

		private class ClientAttackOption : ClientGameOption
		{
			public ClientAttackOption(Client client) : base(client)
			{

			}

			public override void optionSelected()
			{
				client.StartBattle();
			}

			public override string optionTextToSelect()
			{
				return "Аттаковать";
			}

			public override string optionTextWhenPossible()
			{
				return "Что бы аттаковать - напишите 'Аттаковать'";
			}

			public override bool possibilityCheck()
			{
				var allPlayers = client.connectedServer.players;
				var player = client.player;

				foreach (var otherPlayer in allPlayers)
				{
					// Игнорируем проверку на самого себя
					if (otherPlayer.userInfoId == player.userInfoId)
					{
						continue;
					}

					// Проверяем, находится ли другой игрок рядом
					bool isNeighbor =
						(otherPlayer.posX == player.posX - 1 && otherPlayer.posY == player.posY) || // Слева
						(otherPlayer.posX == player.posX + 1 && otherPlayer.posY == player.posY) || // Справа
						(otherPlayer.posX == player.posX && otherPlayer.posY == player.posY - 1) || // Сверху
						(otherPlayer.posX == player.posX && otherPlayer.posY == player.posY + 1);   // Снизу

					if (isNeighbor)
					{
						return true; // Есть игрок рядом
					}
				}

				return false;
			}
		}
	}
}