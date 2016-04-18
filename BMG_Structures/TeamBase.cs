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

		public int TeamId { get; private set; }
		public List<PlayerBase> Players { get; set;}
	}
}
