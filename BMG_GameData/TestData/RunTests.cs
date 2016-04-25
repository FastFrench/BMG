using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_GameData.Building;
using BMG_GameData.Troop;
using BMG_Structures.Buildings;
using BMG_Structures.Troops;
using BMG_Structures.Common;

namespace BMG_GameData.TestData
{
	public static class RunTests
	{
		static public List<TroopTemplateBase> MakeTroopTemplates()
		{
			List<TroopTemplateBase> troops = new List<TroopTemplateBase>();
			int troopId = 5000;			
			troops.Add(new TroopTemplate((TroopEnum)troopId++) { Altitude = AltitudeEnum.Ground, Name = "Warrior", Damage = 50, MaxHP = 500, Range = 10, Speed = 25, Cost = 100, SpaceUsed = 1 });
			troops.Add(new TroopTemplate((TroopEnum)troopId++) { Altitude = AltitudeEnum.Ground, Name = "Goblin", Damage = 35, MaxHP = 200, Range = 10, Speed = 60, Cost = 80, SpaceUsed = 1 });
			troops.Add(new TroopTemplate((TroopEnum)troopId++) { Altitude = AltitudeEnum.Ground, Name = "Archer", Damage = 30, MaxHP = 250, Range = 150, Speed = 20, Cost = 200, SpaceUsed = 1 });
			return troops;
		}
		static public List<BuildingTemplateBase> MakeBuildingTemplates()
		{
			List<BuildingTemplateBase> buildings = new List<BuildingTemplateBase>();
			int buildingId = 1000;
			buildings.Add(new BuildingTemplate((BuildingEnum)buildingId++) { Altitude = AltitudeEnum.Ground, Name = "Wall", Damage = 50, MaxHP = 500, Range = 10, Cost = 100, Delay = 1000, Width = 1 });
			buildings.Add(new BuildingTemplate((BuildingEnum)buildingId++) { Altitude = AltitudeEnum.Ground, Name = "House", Damage = 35, MaxHP = 200, Range = 10, Cost = 80, Delay = 5000, Width = 10 });
			buildings.Add(new BuildingTemplate((BuildingEnum)buildingId++) { Altitude = AltitudeEnum.Ground, Name = "GroundTower", Damage = 30, MaxHP = 250, Range = 150, Cost = 200, Delay = 15000, Width = 2 });
			return buildings;
		}
	}
}
