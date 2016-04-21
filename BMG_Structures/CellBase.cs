using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BMG_Structures.Common;

namespace BMG_Structures
{
	public class CellBase
	{
		public const int MaxTeams = 4;
		public const int TeamMask = 0x0000000F;
		const int bBuildingFly = 4;
		const int bBuildingGround = bBuildingFly+MaxTeams;
		const int bBuildingUnderground = bBuildingGround + MaxTeams;
		const int bTroopFly = bBuildingUnderground + MaxTeams;
		const int bTroopGround = bTroopFly + MaxTeams;
		const int bTroopUnderground = bTroopGround + MaxTeams;
		
		[Flags]
		public enum CellContent
		{
			Empty = 0,

			GroundObstacle = 1 << 1,
			FlyObstacle = 1 << 2,
			UnderGroundObstacle = 1 << 3,

			BuildingFly = 1 << bBuildingFly,
			BuildingFlyMask = TeamMask<<bBuildingFly,
			BuildingGround = 1<<bBuildingGround,
			BuildingGroundMask = TeamMask<<bBuildingGround,
			BuildingUnderground = 1 << bBuildingUnderground,
			BuildingUndergroundMask = TeamMask<<bBuildingUnderground,
			BuildingMask = BuildingFlyMask | BuildingGroundMask | BuildingUndergroundMask,

			TroopFly = 1 << bTroopFly,
			TroopFlyMask = TeamMask<<bTroopFly,
			TroopGround = 1 << bTroopGround,
			TroopGroundMask = TeamMask<<bTroopGround,
			TroopUnderground = 1 << bTroopUnderground,
			TroopUndergroundMask = TeamMask<<bTroopUnderground,
			TroopMask = TroopFlyMask | TroopGroundMask | TroopUndergroundMask,
			
			AllObstacles = GroundObstacle | FlyObstacle | UnderGroundObstacle,
			All = AllObstacles | BuildingMask | TroopMask
		};

		static public CellContent GetTroopFlag(int Team, AltitudeEnum altitude)
		{
			CellContent res = CellContent.Empty;
			if (altitude.HasFlag(AltitudeEnum.Fly))
				res |= (CellContent)((int)CellContent.TroopFly << (Team-1));
			if (altitude.HasFlag(AltitudeEnum.Ground))
				res |= (CellContent)((int)CellContent.TroopGround << (Team-1));
			if (altitude.HasFlag(AltitudeEnum.Underground))
				res |= (CellContent)((int)CellContent.TroopUnderground << (Team-1));
			return res;			
		}

		static public CellContent GetBuildingFlag(int Team, AltitudeEnum altitude)
		{
			CellContent res = CellContent.Empty;
			if (altitude.HasFlag(AltitudeEnum.Fly))
				res |= (CellContent)((int)CellContent.BuildingFly << (Team-1));
			if (altitude.HasFlag(AltitudeEnum.Ground))
				res |= (CellContent)((int)CellContent.BuildingGround << (Team-1));
			if (altitude.HasFlag(AltitudeEnum.Underground))
				res |= (CellContent)((int)CellContent.BuildingUnderground << (Team-1));
			return res;			
		}

		public CellContent Content { get; set; }

	}
}
