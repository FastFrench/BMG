using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Common;

namespace BMG_IA.PathFinder
{
	public class PathFinderMap : Map
	{
		public const int DEFAULT_DISTANCE = 999;

		public PathFinderCellData[] PathFinderCells { get; set; }
		private bool _eightDirs { get; set; }
		public PathFinderMap(int width, int height, bool eightDirs)
			: base(width, height)
		{
			_eightDirs = eightDirs;
		}

		public void ClearLogic()
		{
			// Reset some information about the cells.
			PathFinderCells = new PathFinderCellData[TotalSize];
			InitMovements(_eightDirs ? 8 : 4);
		}


		// Movements is an array of various directions.
		private Point[] _movements { get; set; }

		public void InitMovements(int movementCount)
		{
			// Just do some initializations.
			if (movementCount == 4)
				_movements = dir_x_4;
			else
				_movements = dir_x_8;
		}

		static Point[] dir_x_4 = new Point[]
                {
                    new Point(0, -1), new Point(1, 0), new Point(0, 1), new Point(-1, 0)
                };
		static Point[] dir_x_8 = new Point[]
                {
                    new Point(0, -1), new Point(1, 0), new Point(0, 1), new Point(-1, 0),
                    new Point(-1, 1), new Point(-1, -1), new Point(1, -1), new Point(1, 1)
                };
		
		public IEnumerable<int> ValidMoves(int cell)
		{
			// Return each valid square we can move to.
			foreach (Point movePoint in _movements)
			{
				int NewCellId = GetNeighbourCell(cell, movePoint.X, movePoint.Y);
				//Debug.Assert((NewCellId == CellInfo.CELL_ERROR) || NewCellId >= 0 && NewCellId < CellInfo.NB_CELL);
				if ((NewCellId != CELL_ERROR) && SquareOpen(NewCellId))
				{
					yield return NewCellId;
				}
			}
		}

		public bool SquareOpen(int cellId)
		{
			// A square is open if it is not an obstacle.
			return Cells[cellId].IsWalkable;
		}



	}
}
