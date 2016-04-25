using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures;
using BMG_Structures.Buildings;

namespace BMG_GameData.Building
{
	public class Building : BuildingBase
	{
		new public BuildingTemplate Template { get { return base.Template as BuildingTemplate; } set { base.Template = value; } }

		public Building(BuildingTemplate template, Player player) : base(template, player) 
		{			
		}
	}
}
