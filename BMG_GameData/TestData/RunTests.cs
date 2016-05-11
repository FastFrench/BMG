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
using BMG_Structures;
using BMG_GameData.Effect;

namespace BMG_GameData.TestData
{
	public static class RunTests
	{
		static public IEnumerable<TroopTemplate> MakeTroopTemplates()
		{
			new TroopTemplate() { Altitude = AltitudeEnum.Ground, Name = "Warrior", Damage = 50, MaxHP = 500, Range = 10, Speed = 25, Cost = 100, SpaceUsed = 1 };
			new TroopTemplate() { Altitude = AltitudeEnum.Ground, Name = "Goblin", Damage = 35, MaxHP = 200, Range = 10, Speed = 60, Cost = 80, SpaceUsed = 1 };
			new TroopTemplate() { Altitude = AltitudeEnum.Ground, Name = "Archer", Damage = 30, MaxHP = 250, Range = 150, Speed = 20, Cost = 200, SpaceUsed = 1 };
			return PlaceableFactory.Templates.Values.OfType<TroopTemplate>();
		}
		static public IEnumerable<BuildingTemplate> MakeBuildingTemplates()
		{
			new BuildingTemplate() { Altitude = AltitudeEnum.Ground, Name = "Wall", Damage = 0, MaxHP = 500, Range = 0, Cost = 100, Delay = 1000, Width = 1, HousingCapacity = 0, TargetableAltitude = AltitudeEnum.None };
			new BuildingTemplate() { Altitude = AltitudeEnum.Ground, Name = "House", Damage = 0, MaxHP = 200, Range = 0, Cost = 80, Delay = 5000, Width = 10, HousingCapacity = 10, TargetableAltitude = AltitudeEnum.None };
			new BuildingTemplate() { Altitude = AltitudeEnum.Ground, Name = "Ground Tower", Damage = 30, MaxHP = 250, Range = 350, Cost = 200, Delay = 15000, Width = 2, HousingCapacity = 0, TargetableAltitude = AltitudeEnum.Ground };
			return PlaceableFactory.Templates.Values.OfType<BuildingTemplate>();
		}
		static public IEnumerable<BuildingTemplate> MakeEffectTemplates()
		{
			new EffectTemplate() { Category = EffectTemplateBase.CategoryEnum.Heal, AreaRadius = 1, TargetableAltitude = AltitudeEnum.Fly | AltitudeEnum.Ground, Name = "Small Healing", Damage = 500, AffectBuildings = false, AffectFoes = true, AffectFriends = true, AffectTroops = true, Range = 0, Cost = 100, Delay = 1 };
			new EffectTemplate() { Category = EffectTemplateBase.CategoryEnum.Heal, AreaRadius = 3, TargetableAltitude = AltitudeEnum.Fly | AltitudeEnum.Ground, Name = "Large Healing", Damage = 800, AffectBuildings = false, AffectFoes = true, AffectFriends = true, AffectTroops = true, Range = 0, Cost = 80, Delay = 2 };
			new EffectTemplate() { Category = EffectTemplateBase.CategoryEnum.Damage, AreaRadius = 2, TargetableAltitude = AltitudeEnum.Fly | AltitudeEnum.Ground, Name = "Fireball", Damage = 850, AffectBuildings = true, AffectFoes = true, AffectFriends = true, AffectTroops = true, Range = 0, Cost = 80, Delay = 2 };
			new EffectTemplate() { Category = EffectTemplateBase.CategoryEnum.Teleport, AreaRadius = 1, TargetableAltitude = AltitudeEnum.Fly | AltitudeEnum.Ground, Name = "Teleport", Damage = 0, AffectBuildings = false, AffectFoes = true, AffectFriends = true, AffectTroops = true, Range = 0, Cost = 400, Delay = 1 };
			return PlaceableFactory.Templates.Values.OfType<BuildingTemplate>();
		}
		static public void BuildTemplates()
		{
			PlaceableFactory.FullReset(false);
			MakeTroopTemplates();
			MakeBuildingTemplates();
			MakeEffectTemplates();
			PlaceableFactory.ExportAllTemplates("templates.txt");
		}


	}
}
