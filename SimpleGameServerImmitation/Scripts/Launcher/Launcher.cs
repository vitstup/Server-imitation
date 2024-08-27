using SimpleGameServerImmitation.Scripts.Interfaces;
using SimpleGameServerImmitation.Scripts.States;

namespace SimpleGameServerImmitation.Scripts.Launcher
{
	// У меня есть прямые обращения из лаунчера к базе данных. Хотя понятно в реальном проекте этого бы небыло. 
	// Был бы какой нибудь класс коннектор, который по api получал бы данные и отправлял комманды на сервер. База данных бы взаимодействовала только с сервером.
	// Тут же посколько это будет фактически просто дуюлированием кода и написанием бесмысленных классов с большой тратой времени - это реализованно не было.
	public class Launcher : BasicStateMachine, IConsoleInterfaceShower
	{
		public void ShowInterface()
		{
			new LauncherStartState(this).ShowInterface();
		}

		private class LauncherStartState : ConsoleShowerState
		{
			public LauncherStartState(BasicStateMachine stateMachine) : base(stateMachine)
			{
			}

			public override void StateEntered()
			{
				Console.WriteLine("Добро пожаловать в лаунчер этой прекрасной игры !\nЧто бы войти - напишите 'Вход', что бы зарегистрироваться - напишите 'Регистрация'");
				var input = Console.ReadLine();

				switch (input)
				{
					case "Вход": ChangeState(new LauncherLoginState(stateMachine)); break;
					case "Регистрация": ChangeState(new LauncherRegisterState(stateMachine)); break;
					default: Console.WriteLine("Не распознал ваш текст :(, попробуйте снова !"); ChangeState(new LauncherStartState(stateMachine)); break;
				}
			}
		}

		private class LauncherLoginState : ConsoleShowerState
		{
			public LauncherLoginState(BasicStateMachine stateMachine) : base(stateMachine)
			{
			}

			public override void StateEntered()
			{
				var database = SingletonsHandler.database;

				Console.WriteLine("Введите свой логин");
				var login = Console.ReadLine();

				if (login == "Назад") ChangeState(new LauncherStartState(stateMachine));
				else
				{
					Console.WriteLine("Введите свой пароль");
					var password = Console.ReadLine();

					if (password == "Назад") ChangeState(new LauncherStartState(stateMachine));
					else if (database.usersDatabase.PasswordCheck(login, password, out var user))
					{
						Console.WriteLine("Вы успешно Вошли!");
						SingletonsHandler.currentSession.currentUserInfo = user;
						ChangeState(new LauncherLoggedState(stateMachine));
					}
					else
					{
						Console.WriteLine("Пользователь не существует или введённый пароль не соответсвует настоящему !");
						ChangeState(new LauncherLoginState(stateMachine));
					}
				}
			}
		}

		private class LauncherRegisterState : ConsoleShowerState
		{
			public LauncherRegisterState(BasicStateMachine stateMachine) : base(stateMachine)
			{
			}

			public override void StateEntered()
			{
				var database = SingletonsHandler.database;

				Console.WriteLine("Введите свой логин или вернитесь на главную написав 'Назад' !");
				var login = Console.ReadLine();

				if (login == "Назад") ChangeState(new LauncherStartState(stateMachine));
				else
				{
					if (database.usersDatabase.HaveRegisteredPlayer(login))
					{
						Console.WriteLine("Пользователь уже существует, пожалйста придумайте новый логин или вернитесь на главную страницу!");
						ChangeState(new LauncherRegisterState(stateMachine));
					}
					else
					{
						Console.WriteLine("Введите свой пароль или вернитесь на главную написав 'Назад' !");
						var password = Console.ReadLine();

						if (password == "Назад") ChangeState(new LauncherStartState(stateMachine));
						else if (database.usersDatabase.CreateUser(login, password, out var user))
						{
							Console.WriteLine("Вы успешно зарегистрировались!");
							SingletonsHandler.currentSession.currentUserInfo = user;
							ChangeState(new LauncherLoggedState(stateMachine));
						}
						else
						{
							Console.WriteLine("Что то пошло не так!");
						}
					}
				}
			}
		}

		private class LauncherLoggedState : ConsoleShowerState
		{
			public LauncherLoggedState(BasicStateMachine stateMachine) : base(stateMachine)
			{
			}

			public override void StateEntered()
			{
				Console.WriteLine("Выберите сервер!\nДоступные сервера:");

				var servers = SingletonsHandler.database.serversDatabase.data;

				foreach(var data in servers)
				{
					Console.WriteLine($"Имя сервера: {data.name}, id сервера: {data.id}");
				}

				Console.WriteLine("Что бы выбрать сервер - напишите id сервера");

				var idText = Console.ReadLine();

				if (int.TryParse(idText, out var id))
				{
					if (SingletonsHandler.database.serversDatabase.HaveSuchServer(id))
					{
						Console.WriteLine("Подключение к серверу...");
						SingletonsHandler.client.ConnectToServer(id);
					}
					else
					{
						Console.WriteLine("Пожалуйста введите действительный id");
						ChangeState(new LauncherLoggedState(stateMachine));
					}
				}
				else
				{
					Console.WriteLine("Пожалуйста введите действительный id");
					ChangeState(new LauncherLoggedState(stateMachine));
				}
				
			}
		}

	}
}