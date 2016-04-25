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
		new public BuildingTemplateBase Template { 
			get { return base.Template as BuildingTemplateBase; }
			protected set { base.Template = value; }
		}			
		
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
