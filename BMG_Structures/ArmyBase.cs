using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Troops;

namespace BMG_Structures
{
	public class ArmyBase
	{
		public PlayerBase Player { get; private set; }
		public List<TroopBase> Troops;
		public ArmyBase(PlayerBase player)
		{
			Player = player;
			Troops = new List<TroopBase>();
		}

		public void AddTroop(TroopBase troop)
		{
			Troops.Add(troop);
			Debug.Assert(troop.Player == Player);
		}
	}
}
