using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Buildings;
using BMG_Structures.Common;
using BMG_Structures.Troops;

namespace BMG_Structures
{

	public class PlayerBase
	{
		public string Name { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public int Experience { get; set; }
		public int Victories { get; set; }
		public int Defeats { get; set; }
		public int Level { get; set; }
		public bool Valid { get; set; }

		public ArmyBase Army {get; set;}
		public List<BuildingBase> OwnBuildings { get; private set; }

		public int TeamId
		{
			get
			{
				if (Team != null) return Team.TeamId;
				return 0;
			}
		}

		public TeamBase Team { get; set; }

		public PlayerBase(string login, string password, TeamBase team)
		{
			Name = Login = login;
			Password = password;
			Valid = RetrievePlayer(login, password);
			Team = team;
			Army = new ArmyBase(this);
			OwnBuildings = new List<BuildingBase>();
		}

		protected virtual bool RetrievePlayer(string login, string password)
		{
			return true;
		}

		public IEnumerable<PlaceableBase> GetPlaceables(bool includesBuildings, bool includesTroops)
		{
			if (includesTroops)
				if (includesBuildings)
					return Army.Troops.Concat<PlaceableBase>(OwnBuildings);
				else
					return Army.Troops;
			else
				return OwnBuildings;			
		}

		public TroopBase AddTroop(TroopTemplateBase template)
		{
			TroopBase troop = new TroopBase(template, this);
			Army.AddTroop(troop);
			return troop;
		}

		public TroopBase AddTroop(TroopTemplateBase template, Point point)
		{
			var troop = AddTroop(template);
			if (!point.IsInDeck)
				troop.Drop(point);
			return troop;
		}

		public BuildingBase AddBuilding(BuildingTemplateBase template)
		{
			BuildingBase building = new BuildingBase(template, this);
			OwnBuildings.Add(building);
			return building;
		}

		public BuildingBase AddBuilding(BuildingTemplateBase template, Point point)
		{
			var building = AddBuilding(template);
			if (!point.IsInDeck)
				building.Drop(point);
			return building;
		}

	}
}
