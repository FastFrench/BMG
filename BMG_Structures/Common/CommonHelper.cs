using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Troops;

namespace BMG_Structures.Common
{
	static public class CommonHelper
	{
		static public CellBase.CellContent ImpossibleMoveMask(this AltitudeEnum altitude)
		{
			CellBase.CellContent mask = CellBase.CellContent.AllObstacles;
			if (!altitude.HasFlag(AltitudeEnum.Fly)) mask ^= CellBase.CellContent.FlyObstacle;
			if (!altitude.HasFlag(AltitudeEnum.Ground)) mask ^= CellBase.CellContent.GroundObstacle;
			if (!altitude.HasFlag(AltitudeEnum.Underground)) mask ^= CellBase.CellContent.UnderGroundObstacle;
			return mask;
		}
		static public CellBase.CellContent TargetMask(this AltitudeEnum altitude, int? teamId = null, bool includesTroops = true, bool includesBuildings = true)
		{
			CellBase.CellContent mask = CellBase.CellContent.Empty;
						
			if (includesBuildings)
			{
				if (!altitude.HasFlag(AltitudeEnum.Fly)) 
				{
					mask |= CellBase.CellContent.BuildingFlyMask;
					if (teamId != null)
						mask ^= CellBase.GetBuildingFlag(teamId.Value, AltitudeEnum.Fly);
				}
				if (!altitude.HasFlag(AltitudeEnum.Ground))
				{
					mask |= CellBase.CellContent.BuildingGroundMask;
					if (teamId != null)
						mask ^= CellBase.GetBuildingFlag(teamId.Value, AltitudeEnum.Ground);
				}
				if (!altitude.HasFlag(AltitudeEnum.Underground))
				{
					mask |= CellBase.CellContent.BuildingUndergroundMask;
					if (teamId != null)
						mask ^= CellBase.GetBuildingFlag(teamId.Value, AltitudeEnum.Underground);
				}
			}

			if (includesTroops)
			{
				if (!altitude.HasFlag(AltitudeEnum.Fly)) 
				{
					mask |= CellBase.CellContent.TroopFlyMask;
					if (teamId != null)
						mask ^= CellBase.GetTroopFlag(teamId.Value, AltitudeEnum.Fly);
				}
				if (!altitude.HasFlag(AltitudeEnum.Ground))
				{
					mask |= CellBase.CellContent.TroopGroundMask;
					if (teamId != null)
						mask ^= CellBase.GetTroopFlag(teamId.Value, AltitudeEnum.Ground);
				}
				if (!altitude.HasFlag(AltitudeEnum.Underground))
				{
					mask |= CellBase.CellContent.TroopUndergroundMask;
					if (teamId != null)
						mask ^= CellBase.GetTroopFlag(teamId.Value, AltitudeEnum.Underground);
				}
			}
			return mask;
		}

		static public CellBase.CellContent ImpossibleMoveMask(this TroopBase troop)
		{
			return troop.Altitude.ImpossibleMoveMask();
		}

		static public CellBase.CellContent TargetMask(this PlaceableBase placeable, bool includeTroops = true, bool includeBuildings = true)
		{
			return placeable.TargetableAltitudes.TargetMask(placeable.Player.TeamId, includeTroops, includeBuildings );
		}

	}
}
