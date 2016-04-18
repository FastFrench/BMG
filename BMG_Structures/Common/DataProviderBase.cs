using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Buildings;
using BMG_Structures.Troops;

namespace BMG_Structures.Common
{
	public abstract class DataProviderBase : IDataProviderBase
	{
		protected DataProviderBase()
		{
			troopTypes = BuildTroopTemplates();
			buildingTypes = BuildBuildingTemplates();
		}

		protected abstract List<TroopTemplateBase> BuildTroopTemplates();
		protected abstract List<BuildingTemplateBase> BuildBuildingTemplates();

		protected List<TroopTemplateBase> troopTypes { get; set; }
		protected List<BuildingTemplateBase> buildingTypes { get; set; }

		public List<TroopTemplateBase> TroopTypes { get { return troopTypes; } }
		public List<BuildingTemplateBase> BuildingTypes { get { return buildingTypes; } }
	}
}
