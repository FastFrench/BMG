using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures;
using BMG_Structures.Common;
using BMG_Structures.Troops;

namespace BMG_IA.Common
{
	abstract public class TroopAI : CommonAI
	{
		override public Point MoveTo(BattleFieldBase battleField, TroopBase attacker)
		{
			// If the troop is not deployed or dead, then return default InDeck position. 
			if (attacker == null || attacker.CurrentPosition.IsInDeck || attacker.CurrentHP <= 0) return Point.InDeck;
			return battleField.FindClosestCellInRange(attacker.CurrentPosition, attacker.VisionRange, attacker.MinAttackRange, true, true, attacker.Player.TeamId, true);
		}

	}
}
