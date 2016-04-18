using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Common;

namespace BMG_IA.Common
{
	public class AIBase : AIBase
	{
		public enum MoveEnum { ClosestTroop, ClosestBuilding, ClosestAny, WeakestBuildingAtRange, WeakestTroopAtRange, WeakestAnyAtRange, StayAtRange }		
		public enum TargetFocus { ClosestTroop, ClosestBuilding, ClosestAny, WeakestBuilding, WeakestTroop, WeakestAny, MostWoundedBuilding, MostWoundedTroop, MostWoundedAny }

		bool IgnoreTroops { get; set;}
		bool IgnoreBuildings { get; set; }	
	}
}
