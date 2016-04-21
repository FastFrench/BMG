using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		public PlaceableBase CurrentAttackTarget { get; set;}
		private readonly static TimeSpan DefaultAttackTargetUpdateDelay = new TimeSpan(0, 0, 0, 0, 500);

		protected readonly static TimeSpan DefaultFreezeDelayAtStart = new TimeSpan(0, 0, 0, 1, 0); // Stay 1 second freezed when dropped
		
		virtual protected TimeSpan AttackTargetUpdateDelay { get; set; }
		protected Stopwatch AttackRetargetSW { get; set; }

		public PlaceableBase()
		{
			CurrentAttackTarget = null;
			VisionRange = DefaultVisionRange;
			AttackRetargetSW = Stopwatch.StartNew();
			AttackTargetUpdateDelay = DefaultFreezeDelayAtStart;
		}

		public virtual Point MoveTo(BattleFieldBase battleField)
		{
			return Point.InDeck;			
		}

		public virtual PlaceableBase Target(BattleFieldBase battleField)
		{
			if (CurrentAI == null) return null; // No AI, no fight !
			// If the current target is still in range, then keep attacking it

			return CurrentAI.Target(battleField, this);
		}
	}
}
