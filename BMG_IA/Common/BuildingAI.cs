﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Common;

namespace BMG_IA.Common
{
	public class BuildingAI : AIBase
	{
		public override Point MoveTo(BMG_Structures.BattleFieldBase battleField, BMG_Structures.Troops.TroopBase attacker)
		{
			throw new NotImplementedException();
		}
		public override PlaceableBase Target(BMG_Structures.BattleFieldBase battleField, PlaceableBase attacker)
		{
			return battleField.FindClosestCellInRange
		}
	}
}
