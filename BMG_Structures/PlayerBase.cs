using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public int TeamId
		{
			get
			{
				if (Team != null) return Team.TeamId;
				return 0;
			}
		}

		public TeamBase Team { get; set; }

		PlayerBase(string name, TeamBase team)
		{
			Name = name;
			Team = team;
		}

	}
}
