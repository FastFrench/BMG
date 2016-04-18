using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Common;

namespace BMG_Structures.Buildings
{
	public class BuildingBase : PlaceableBase
	{
		public int CurrentHP { get; set; }
		public Point CurrentPosition { get; set; }
		public AIBase CurrentAI { get; set; }
		public BuildingTemplateBase Template { get; protected set; }
		public PlayerBase Player { get; protected set; }

		public BuildingBase()
		{
			CurrentPosition = Point.InDeck;
		}

		public BuildingBase(BuildingTemplateBase template, PlayerBase player)
			: this()
		{
			Template = template;
			CurrentHP = template.MaxHP;
			Player = player;
		}
	}
}
