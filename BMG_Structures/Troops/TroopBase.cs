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
		public int CurrentHP { get; set; }
		public AIBase CurrentAI { get; set; }
		public TroopTemplateBase Template { get; protected set; }
		public PlayerBase Player { get; protected set; }
		virtual public AltitudeEnum TargetableAltitude { get; protected set; } 
		virtual 
		public TroopBase()
		{
			CurrentPosition = Point.InDeck;			
		}

		public TroopBase(TroopTemplateBase template, PlayerBase player) : this()
		{
			Template = template;
			CurrentHP = template.MaxHP;
			Player = player;
		}
	}
}
