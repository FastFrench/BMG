using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Buildings;

namespace BMG_GameData.Building
{
	public class BuildingTemplate : BuildingTemplateBase
	{
		int HousingCapacity { get; set; }
		public BuildingTemplate()
			: base()
		{
		}
	}
}
