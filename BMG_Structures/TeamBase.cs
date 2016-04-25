using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMG_Structures
{
	public class TeamBase
	{
		public PlayerBase this[string login]
		{
			get
			{
				return Players.FirstOrDefault(pl => pl.Login == login);
			}
		}

		static private int teamIdCounter = 0;
		public TeamBase()
		{
			TeamId = ++teamIdCounter;
			Players = new List<PlayerBase>();
		}

		internal static void ResetCounter() { teamIdCounter = 0; }
		
		protected PlayerBase CreatePlayer(string login, string password)
		{
			return new PlayerBase(login, password, this);
		}

		public virtual PlayerBase AddPlayer(string login, string password)
		{
			PlayerBase newPlayer = CreatePlayer(login, password);
			if (newPlayer == null || !newPlayer.Valid) return null;
			Players.Add(newPlayer);
			newPlayer.Team = this;
			return newPlayer;
		}

		public virtual bool MoveExistingPlayerIntoThisTeam(PlayerBase player)
		{
			if (player == null || player.Team == this) return false;
			if (player.Team != null)
				player.Team.RemovePlayer(player);
			player.Team = this;
			Players.Add(player);
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
