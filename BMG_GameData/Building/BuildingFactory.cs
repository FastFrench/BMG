using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures;
using BMG_Structures.Buildings;
using BMG_Structures.Common;

namespace BMG_GameData.Building
{
	public class BuildingFactory
	{
		BuildingTemplate GetTemplate(int templateId)
		{
			TemplateBase template = null;
			if (PlaceableFactory.Templates.TryGetValue(templateId, out template) && template is BuildingTemplate)
				return template as BuildingTemplate;
			return null;
		}
		Building CreateBuilding(int templateId, Player player)
		{
			BuildingTemplate template = GetTemplate(templateId);
			if (template == null) return null;
			return new Building(template, player);
		}
	}
}
