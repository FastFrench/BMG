using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Buildings;

namespace BMG_GameData.Building
{
	public class BuildingFactory
	{
		BuildingTemplate CreateTemplate(BuildingEnum type)
		{
			return new BuildingTemplate(type);
		}
		Building CreateBuilding(BuildingEnum type)
		{
			return new Building(CreateTemplate(type));
		}
	}
}
