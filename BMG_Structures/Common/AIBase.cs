using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Troops;

namespace BMG_Structures.Common
{
	/// <summary>
	/// This interface is designed to manipulate AIBase objects without any reference to BMG_AI
	/// Any AI implements this interface
	/// </summary>
	public abstract class AIBase
	{
		public enum MoveEnum { ClosestTroop, ClosestBuilding, ClosestAny, WeakestBuildingAtRange, WeakestTroopAtRange, WeakestAnyAtRange, StayAtRange }
		public enum TargetFocus { ClosestTroop, ClosestBuilding, ClosestAny, WeakestBuilding, WeakestTroop, WeakestAny, MostWoundedBuilding, MostWoundedTroop, MostWoundedAny }

		public virtual bool IgnoreTroops { get; set; }
		public virtual bool IgnoreBuildings { get; set; }
		
		abstract public Point MoveTo(BattleFieldBase battleField, TroopBase attacker);
		abstract public PlaceableBase Target(BattleFieldBase battleField, PlaceableBase attacker);
	}
}
