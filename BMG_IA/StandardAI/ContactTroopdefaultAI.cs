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
		override public Point MoveTo(BattleFieldBase battleField, PlayerBase player, TroopBase attacker)
		{
			// If the troop is not deployed or dead, then return default InDeck position. 
			if (attacker == null || attacker.CurrentPosition.IsInDeck || attacker.CurrentHP <= 0) return Point.InDeck;
			Point CurrentPosition = attacker.CurrentPosition;
			battleField.CellsAtRange
		}
		override public PlaceableBase Target(BattleFieldBase battleField, PlayerBase player, PlaceableBase attacker)
		{

		}

	}
}
