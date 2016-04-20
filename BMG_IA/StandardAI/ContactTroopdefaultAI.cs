using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_IA.Common;
using BMG_Structures;
using BMG_Structures.Common;
using BMG_Structures.Troops;

namespace BMG_IA.StandardAI
{
	public class ContactTroopdefaultAI : TroopAI
	{
		override public Point MoveTo(BattleFieldBase battleField, TroopBase attacker)
		{
			// If the troop is not deployed or dead, then return default InDeck position. 
			if (attacker == null || attacker.CurrentPosition.IsInDeck || attacker.CurrentHP <= 0) return Point.InDeck;
			Point CurrentPosition = attacker.CurrentPosition;
			var dest = battleField.FindClosestCellInRange(CurrentPosition, attacker.VisionRange, 0, true, true, attacker.Player.TeamId, true);
			if (dest.IsInDeck)
			{

			}
		}
		override public PlaceableBase Target(BattleFieldBase battleField, PlaceableBase attacker)
		{
			battleField.FindClosestCellInRange(CurrentPosition, attacker.VisionRange, 0, true, true, attacker.Player.TeamId, true);
		}

	}
}
