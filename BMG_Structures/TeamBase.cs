using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMG_Structures
{
	public class TeamBase
	{
		static private int teamIdCounter = 0;
		public TeamBase()
		{
			TeamId = ++teamIdCounter;
			Players = new List<PlayerBase>();
		}

		protected PlayerBase CreatePlayer(string login, string password)
		{
			return new PlayerBase(login, password, this);
		}

		public virtual bool AddPlayer(string login, string password)
		{
			PlayerBase newPlayer = CreatePlayer(login, password);
			if (newPlayer == null || !newPlayer.Valid) return false;
			Players.Add(newPlayer);
			return true;
		}

		public virtual bool RemovePlayer(PlayerBase player)
		{
			if (player.Team != this) return false;
			player.Team = null;
			return Players.Remove(player);
		}

		public int TeamId { get; private set; }
		public List<PlayerBase> Players { get; set;}
	}
}
