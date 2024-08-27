// Тут простая реализация классов игры
namespace SimpleGameServerImmitation.Scripts
{
	public class Map
	{
		public int sizeX, sizeY;

		public Map(int sizeX, int sizeY)
		{
			this.sizeX = sizeX;
			this.sizeY = sizeY;
		}
	}

	public class Player
	{
		public int userInfoId;
		public int posX, posY;
		public int maxHp, currentHp;
		public int damage;
		public int money;
	}
}