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
			CellBase.CellContent mask = CellBase.CellContent.TotalObstacles;
			if (!altitude.HasFlag(AltitudeEnum.Fly)) mask ^= CellBase.CellContent.FlyObstacle;
			if (!altitude.HasFlag(AltitudeEnum.Ground)) mask ^= CellBase.CellContent.GroundObstacle;
			if (!altitude.HasFlag(AltitudeEnum.Underground)) mask ^= CellBase.CellContent.UnderGroundObstacle;
			return mask;
		}
		static public CellBase.CellContent ImpossibleMoveMask(this TroopBase troop)
		{
			return troop.Altitude.ImpossibleMoveMask();
		}
		static public CellBase.CellContent TargetMask(this PlaceableBase placeable)
		{
			return placeable.TargetableAltitudes.ImpossibleMoveMask();
		}

	}
}
