using System;
using System.Collections.Generic;
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

		virtual public int BaseSpeed { get { return 0; } }
		public TroopBase()
		{
			CurrentPosition = Point.InDeck;			
			VisionRange = DefaultVisionRange;
		}

		public TroopBase(TroopTemplateBase template, PlayerBase player) : this()
		{
			Template = template;
			CurrentHP = template.MaxHP;
			Player = player;
		}
	}
}
