namespace SimpleGameServerImmitation.Scripts.Databases
{
	public class UsersDatabase : TypedDatabase<UserInfo>
	{
		public bool HaveRegisteredPlayer(string login)
		{
			return data.Any(user => user.login == login);
		}

		public bool PasswordCheck(string login, string password)
		{
			return data.Any(user => user.login == login && user.password == password);
		}

		public bool PasswordCheck(string login, string password, out UserInfo user)
		{
			user = data.FirstOrDefault(user => user.login == login && user.password == password);
			return user != null;
		}

		public bool CreateUser(string login, string password)
		{
			if (HaveRegisteredPlayer(login)) return false;

			int newUserId = data.Any() ? data.Max(user => user.id) + 1 : 1;
			var newUser = new UserInfo { id = newUserId, login = login, password = password };
			data.Add(newUser);

			return true;
		}

		public bool CreateUser(string login, string password, out UserInfo user)
		{
			if (HaveRegisteredPlayer(login))
			{
				user = null;
				return false;
			}

			int newUserId = data.Any() ? data.Max(user => user.id) + 1 : 1;
			user = new UserInfo { id = newUserId, login = login, password = password };
			data.Add(user);

			return true;
		}

		public UserInfo GetUserById(int id)
		{
			return data.FirstOrDefault(user => user.id == id);
		}
	}

	public class UserInfo
	{
		public int id;
		public string login;
		public string password;
	}
}