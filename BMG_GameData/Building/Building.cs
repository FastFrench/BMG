using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Buildings;

namespace BMG_GameData.Building
{
	public class Building : BuildingBase
	{
		public BuildingEnum TypeId { get { return (BuildingEnum)Template.TemplateId; } }
		
		public Building(BuildingTemplate template)
		{
			Template = template;			
		}
	}
}
