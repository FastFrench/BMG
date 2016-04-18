using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_GameData.TestData;
using BMG_Structures.Common;

namespace BMG_GameData
{
    public class DataProvider : DataProviderBase
    {
			public DataProvider() : base()
			{

			}

			protected override List<BMG_Structures.Buildings.BuildingTemplateBase> BuildBuildingTemplates()
			{
				return RunTests.MakeBuildingTemplates();
			}

			protected override List<BMG_Structures.Troops.TroopTemplateBase> BuildTroopTemplates()
			{
				return RunTests.MakeTroopTemplates();
			}
    }
}
