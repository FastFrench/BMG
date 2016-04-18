using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Buildings;

namespace BMG_Structures
{

	public class PlayerBase
	{
		public string Name { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public int Experience { get; set; }
		public int Victories { get; set; }
		public int Defeats { get; set; }
		public int Level { get; set; }
		public bool Valid { get; set; }

		public ArmyBase Army {get; set;}
		public List<BuildingBase> OwnBuildings { get; private set; }

		public int TeamId
		{
			get
			{
				if (Team != null) return Team.TeamId;
				return 0;
			}
		}

		public TeamBase Team { get; set; }

		public PlayerBase(string login, string password, TeamBase team)
		{
			Login = login;
			Password = password;
			Valid = RetrievePlayer(login, password);
			Team = team;
		}

		protected virtual bool RetrievePlayer(string login, string password)
		{
			return true;
		}

	}
}
