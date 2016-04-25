using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using BMG_Structures.Buildings;
using BMG_Structures.Common;
using BMG_Structures.Troops;

namespace BMG_Structures
{
	public static class PlaceableFactory
	{
		public static Dictionary<int, TemplateBase> Templates = new Dictionary<int, TemplateBase>();

		public static PlaceableBase CreatePlaceable(int templateId, PlayerBase player)
		{
			TemplateBase template = null;
			if (Templates.TryGetValue(templateId, out template))
			{
				if (template is TroopTemplateBase)
					return new TroopBase(template as TroopTemplateBase, player);
				if (template is BuildingTemplateBase)
					return new BuildingBase(template as BuildingTemplateBase, player);
			}
			Debug.Assert(false);
			return null;
		}

		public static void FullReset()
		{
			Templates.Clear();
			PlaceableBase.ResetCounter();
			TemplateBase.ResetCounter();
			TeamBase.ResetCounter();
		}

		public static bool ExportAllTemplates(string filename)
		{
			var types = Templates.Values.ToList().Select(elt => elt.GetType()).Distinct().Where(typ=>typ.Name != typeof(TemplateBase).Name).ToArray();
			XmlSerializer xs = new XmlSerializer(typeof(List<TemplateBase>), types);
			using (StreamWriter wr = new StreamWriter(filename))
			{
				xs.Serialize(wr, Templates.Values.ToList());
			}
			return true;
		}
	}
}

