using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures;

namespace BMG_GameData
{
	public class Player : PlayerBase
	{
		public Player(string login, string password, TeamBase team) : base(login, password, team)
		{

		}
	}
}
