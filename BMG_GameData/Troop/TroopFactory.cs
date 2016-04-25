using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures;
using BMG_Structures.Buildings;
using BMG_Structures.Common;

namespace BMG_GameData.Troop
{
	public class TroopFactory
	{
		TroopTemplate GetTemplate(int templateId)
		{
			TemplateBase template = null;
			if (PlaceableFactory.Templates.TryGetValue(templateId, out template) && template is TroopTemplate)
				return template as TroopTemplate;
			return null;
		}
		Troop CreateBuilding(int templateId, Player player)
		{
			TroopTemplate template = GetTemplate(templateId);
			if (template == null) return null;
			return new Troop(template, player);
		}
	}
}
