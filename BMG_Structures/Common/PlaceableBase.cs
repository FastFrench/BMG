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
		const int DefaultVisionRange = 100;		
		public Point CurrentPosition { get; protected set; }
		virtual public AltitudeEnum Altitude { get; protected set; }
		virtual public AltitudeEnum TargetableAltitudes { get { return Altitude; } }
		public PlayerBase Player { get; protected set; }
		public AIBase CurrentAI { get; set; }
		public int CurrentHP { get; set; }
		virtual public int VisionRange { get; protected set;}
		virtual public int MinAttackRange { get { return 0; } }
		virtual public int MaxAttackRange { get { return 1; } }
		public PlaceableBase()
		{
			VisionRange = DefaultVisionRange;
		}
		public virtual Point MoveTo(BattleFieldBase battleField)
		{
			if (CurrentAI == null || !(this is TroopBase)) return Point.InDeck;
			return CurrentAI.MoveTo(battleField, this as TroopBase);
		}

		public virtual PlaceableBase Target(BattleFieldBase battleField)
		{
			if (CurrentAI == null) return null;
			return CurrentAI.Target(battleField, this);
		}
	}
}
