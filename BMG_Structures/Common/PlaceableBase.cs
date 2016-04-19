using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Troops;

namespace BMG_Structures.Common
{
	public class PlaceableBase
	{
		public Point CurrentPosition { get; protected set; }
		virtual public AltitudeEnum Altitude { get; protected set; }
		public PlayerBase Player { get; protected set; }
		public AIBase CurrentAI { get; set; }
		public int CurrentHP { get; set; }

		public virtual Point MoveTo(BattleFieldBase battleField)
		{
			if (CurrentAI == null || !(this is TroopBase)) return Point.InDeck;
			return CurrentAI.MoveTo(battleField, Player, this as TroopBase);
		}

		public virtual PlaceableBase Target(BattleFieldBase battleField)
		{
			if (CurrentAI == null) return null;
			return CurrentAI.Target(battleField, Player, this);
		}
	}
}
