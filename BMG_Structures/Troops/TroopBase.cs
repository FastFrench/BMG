using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Common;

namespace BMG_Structures.Troops
{
	public class TroopBase : PlaceableBase
	{
		public const int DefaultVisionRange = 100;
		virtual public TroopTemplateBase Template { get; protected set; }
		virtual public AltitudeEnum TargetableAltitude { get; protected set; }

		private static readonly TimeSpan DefaultMoveTargetUpdateDelay = new TimeSpan(0, 0, 0, 0, 200);
		virtual protected TimeSpan MoveTargetUpdateDelay { get; set; }
		protected Stopwatch MoveRetargetSW { get; set; }
		public Point CurrentDestination { get; set; }
		
		virtual public int BaseSpeed { get { return 0; } }
		public TroopBase()
		{
			CurrentPosition = Point.InDeck;			
			VisionRange = DefaultVisionRange;
			MoveRetargetSW = Stopwatch.StartNew();
			MoveTargetUpdateDelay = DefaultFreezeDelayAtStart;
			CurrentDestination = Point.InDeck;			
		}

		public TroopBase(TroopTemplateBase template, PlayerBase player) : this()
		{
			Template = template;
			CurrentHP = template.MaxHP;
			Player = player;
		}

		/// <summary>
		/// Returns the current destination, and update it as needed.
		/// </summary>
		/// <param name="battleField"></param>
		/// <returns></returns>
		public virtual Point MoveTo(BattleFieldBase battleField)
		{
			if (CurrentAI == null) return Point.InDeck;
			if (CurrentAttackTarget != null && !CurrentAttackTarget.CurrentPosition.IsInDeck)
				return CurrentAttackTarget.CurrentPosition;
			if (MoveRetargetSW.Elapsed > MoveTargetUpdateDelay)
			{
				// We are not currently moving, so search for a possible destination. 
				MoveRetargetSW.Restart();
				MoveTargetUpdateDelay = DefaultMoveTargetUpdateDelay;
				CurrentDestination = CurrentAI.MoveTo(battleField, this as TroopBase);
			}
			return CurrentDestination;
		}

	}
}
