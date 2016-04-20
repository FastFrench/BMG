﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMG_Structures
{
	public class CellBase
	{
		public const int MaxTeams = 12;
		public const int TeamMask = 0x003F;
		const int bBuilding = 4;
		const int bTroop = bBuilding + MaxTeams;
		
		[Flags]
		public enum CellContent
		{
			Empty = 0,
			GroundObstacle = 1 << 1,
			FlyObstacle = 1 << 2,
			UnderGroundObstacle = 1 << 3,
			BuildingT1 = 1 << bBuilding,
			BuildingMask = TeamMask<<bBuilding,
			TroopT1 = 1 << bTroop,
			TroopMask = TeamMask << bTroop,
			TotalObstacles = GroundObstacle | FlyObstacle | UnderGroundObstacle,
			AllExceptBuildings = TotalObstacles | BuildingMask,
			AllExceptTroops = TotalObstacles | BuildingMask
		};

		static public CellContent GetTroopFlag(int Team)
		{
			return (CellContent)((int)CellContent.TroopT1 << (Team-1));
		}

		static public CellContent GetBuildingFlag(int Team)
		{
			return (CellContent)((int)CellContent.BuildingT1 << (Team-1));
		}

		public CellContent Content { get; set; }

	}
}
