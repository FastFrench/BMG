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
		virtual public TroopTemplateBase Template { get; protected set; }
		virtual public AltitudeEnum TargetableAltitude { get; protected set; } 
		
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
