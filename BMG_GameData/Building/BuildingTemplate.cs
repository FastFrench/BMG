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
		public BuildingTemplate(BuildingEnum type)
		{
			TemplateId = (int)type;
		}
	}
}
