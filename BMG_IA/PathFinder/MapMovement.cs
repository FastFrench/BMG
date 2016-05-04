using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace BMG_IA.PathFinder
{
	public class MapMovement
	{
		Map cells;
		public MapMovement(Map _cells)
		{
			cells = _cells;
		}
		

		/// <summary>
		/// Give each cell (included the starting cell) of the path as input, and compute packed cell array for GameMapMovementRequestMessage. 
		/// </summary>
		/// <param name="path">Minimum size = 2 (source and dest cells)</param>
		/// <returns></returns>
		public int[] PackPath(int[] path)
		{
			Debug.Assert(path.Length > 1); // At least source and dest cells          
			List<int> PackedPath = new List<int>();
			if (path.Length < 2) return PackedPath.ToArray();
			DirectionsEnum PreviousOrientation = DirectionsEnum.DIRECTION_EAST;
			DirectionsEnum Orientation = DirectionsEnum.DIRECTION_EAST;
			int PreviousCellId = path[0];
			for (int NoCell = 1; NoCell < path.Length; NoCell++)
			{
				int cellid = path[NoCell];
				//Debug.Assert(cellid >= 0 && cellid < CellInfo.NB_CELL);

				Orientation = cells.GetOrientation(PreviousCellId, cellid);
				if (NoCell == 1 || (Orientation != PreviousOrientation) || NoCell == (path.Length - 1)) // Odd, but first step is always packed
				{
					PackedPath.Add(cellid | ((int)Orientation) << 12);
					PreviousOrientation = Orientation;
				}

				PreviousCellId = cellid;
			}
			return PackedPath.ToArray();
		}
	}
}
