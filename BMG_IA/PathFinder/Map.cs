using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Common;

namespace BMG_IA.PathFinder
{
	public class Map
	{
		public const int CELL_ERROR = -1;
		
		public CellInfo[] Cells { get; set; }
		public int TotalSize { get; set; }
		public Map(int width, int height)
		{
			Width = width;
			Height = height;
			TotalSize = Width * Height;
			Cells = new CellInfo[TotalSize];
			//for (int i = 0; i < TotalSize; i++)
			//	Cells[i] = new CellInfo(true, true);
		}

		public CellInfo this[int index]
		{
			get
			{
				if (index < 0 || index >= TotalSize) return CellInfo.EmptyCell;
				return Cells[index];
			}
			set
			{
				if (index < 0 || index >= TotalSize) return;
				Cells[index] = value;
			}
		}
		public CellInfo this[int x, int y]
		{
			get
			{
				if (x < 0 || x >= Width || y < 0 || y >= Height) return CellInfo.EmptyCell;
				return Cells[x + y * Width];
			}
			set
			{
				if (x < 0 || x >= Width || y < 0 || y >= Height) return;
				Cells[x + y * Width] = value;
			}
		}
		public CellInfo this[Point pt]
		{
			get
			{
				if (pt.IsInDeck) return CellInfo.EmptyCell;
				return Cells[pt.X + pt.Y * Width];
			}
			set
			{
				Cells[pt.X + pt.Y * Width] = value;
			}
		}

		public int Width { get; private set; }
		public int Height { get; private set; }

		// X is from 0 to Width-1
		// Y is from 0 to Height-1
		public int GetIdFromPos(Point pt)
		{
			if (pt.IsInDeck) return CELL_ERROR;
			return pt.X + pt.Y * Width;
		}

		public int GetIdFromPosSafe(Point pt)
		{
			if (pt.IsInDeck || pt.X < 0 || pt.X >= Width || pt.Y < 0 || pt.Y >= Height) return CELL_ERROR;
			return pt.X + pt.Y * Width;
		}

		public int GetIdFromPos(int x, int y)
		{
			return x + y * Width;
		}

		public int GetIdFromPosSafe(int x, int y)
		{
			if (x < 0 || x >= Width || y < 0 || y >= Height) return CELL_ERROR;
			return x + y * Width;
		}

		public Point GetPointFromId(int id)
		{
			if (id == CELL_ERROR) return Point.InDeck;
			return new Point(id % Width, id / Width);
		}

		public int GetNeighbourCell(int cellId, int dx, int dy)
		{
			return GetIdFromPosSafe(xfromid(cellId) + dx, yfromid(cellId) + dy);
		}

		public int GetNeighbourCell(Point point, int dx, int dy)
		{
			return GetIdFromPosSafe(point.X + dx, point.Y + dy);
		}

		int xfromid(int id)
		{
			if (id < 0) return -1;
			return id % Width;
		}
		int yfromid(int id)
		{
			if (id < 0) return -1;
			return id / Width;
		}

		/// <summary>
		/// Enumerates all cells within a given distance from cellId
		/// </summary>
		/// <param name="cellId"></param>
		/// <param name="distanceMax"></param>
		/// <returns></returns>
		public IEnumerable<int> FindCellsAround(int cellId, int distanceMax, bool WalkableOnly, bool LOSNeeded = false)
		{
			List<int> result = new List<int>();

			int x = xfromid(cellId);
			int y = yfromid(cellId);

			for (int px = x - distanceMax; px <= x + distanceMax; px++)
				if (px >= 0 && px < Width)
					for (int py = y - distanceMax; py <= y + distanceMax; py++)
						if (py >= 0 && py < Height)
							//if (px >= 0 && py >= 0 && px <= CellInfo.MAP_SIZE && py <= CellInfo.MAP_SIZE) // Within map
							if ((Math.Abs(x - px) + Math.Abs(y - py)) <= distanceMax) // Close enough
							{
								int newCellId = GetIdFromPos(px, py);
								if (!WalkableOnly || Cells[newCellId].IsWalkable)
									if (!LOSNeeded || CanBeSeen(cellId, newCellId))
										yield return newCellId;
							}
		}

		/// <summary>
		/// Returns straight path from cell1 to cell2 (excluding cell1, including cell2).
		/// </summary>
		/// <param name="cell1"></param>
		/// <param name="cell2"></param>
		/// <returns></returns>
		public IEnumerable<int> GetStraightPath(int cell1, int cell2)
		{		
			int x1 = xfromid(cell1); int x2 = xfromid(cell2);
			int y1 = yfromid(cell1); int y2 = yfromid(cell2);
			if (x1 == x2 && y1 == y2) 
			{
				yield break;				
			}
			int dx = Math.Abs(x2 - x1);
			int dy = Math.Abs(y2 - y1);
			if (dx > dy)
			{
				int deltaX = (x2 - x1) / dx;
				double deltaY = ((double)(y2 - y1)) / dx;
				for (int n = 1; n <= dx; n++)
					yield return GetIdFromPos(x1 + n * deltaX, (int)(y1 + n * deltaY));					
			}
			else
			{
				int deltaY = (y2 - y1) / dy;
				double deltaX = ((double)(x2 - x1)) / dy;
				for (int n = 1; n <= dy; n++)
					yield return GetIdFromPos((int)(x1 + n * deltaX), y1 + n * deltaY);					
			}			
		}


		/// <summary>
		/// Says if Cell1 can see Cell2.
		/// (Quite optimized)
		/// </summary>
		/// <param name="cell1"></param>
		/// <param name="cell2"></param>
		/// <returns></returns>
		public bool CanBeSeen(int cell1, int cell2)
		{

			int x1 = xfromid(cell1); int x2 = xfromid(cell2);
			int y1 = yfromid(cell1); int y2 = yfromid(cell2);
			if (x1 == x2 && y1 == y2) return true; // On same cell
			CellInfo info;

			int dx = Math.Abs(x2 - x1);
			int dy = Math.Abs(y2 - y1);
			if (dx > dy)
			{
				int deltaX = (x2 - x1) / dx;
				double deltaY = ((double)(y2 - y1)) / dx;
				for (int n = 0; n <= dx; n++)
				{
					int x = x1 + n * deltaX;
					int yb = (int)(y1 + n * deltaY - 0.5); // Truncated toward 0
					for (int y = yb; y <= yb + 1; y++)
					{
						info = this[x, y];
						if (info.Error || info.AllowLOS) continue;
						// If one obstacle on the LOS, returns false
						if (TooCloseFromSegment(x, y, x1, y1, x2, y2))
							return false;
					}
				}
			}
			else
			{
				int deltaY = (y2 - y1) / dy;
				double deltaX = ((double)(x2 - x1)) / dy;
				for (int n = 0; n <= dy; n++)
				{
					int y = y1 + n * deltaY;
					int xb = (int)(x1 + n * deltaX - 0.5); // Truncated toward 0
					for (int x = xb; x <= xb + 1; x++)
					{
						info = this[x, y];
						if (info.Error || info.AllowLOS) continue;
						// If one obstacle on the LOS, returns false
						if (TooCloseFromSegment(x, y, x1, y1, x2, y2))
							return false;
					}
				}
			}
			return true;
		}


		/// <summary>
		/// Check if distance to the segment is less than 0.71 (stricly under sqrt(2)/2).
		/// </summary>
		/// <param name="cx"></param>
		/// <param name="cy"></param>
		/// <param name="ax"></param>
		/// <param name="ay"></param>
		/// <param name="bx"></param>
		/// <param name="by"></param>
		/// <returns></returns>
		private bool TooCloseFromSegment(int cx, int cy, int ax, int ay,
																							int bx, int by)
		{
			const double MIN_DISTANCE = 0.71;
			//
			// find the distance from the point (cx,cy) to the line
			// determined by the points (ax,ay) and (bx,by)
			//
			// distanceSegment = distance from the point to the line segment
			// distanceLine = distance from the point to the line (assuming
			//                                        infinite extent in both directions
			//

			/*

			Subject 1.02: How do I find the distance from a point to a line?


					Let the point be C (Cx,Cy) and the line be AB (Ax,Ay) to (Bx,By).
					Let P be the point of perpendicular projection of C on AB.  The parameter
					r, which indicates P's position along AB, is computed by the dot product 
					of AC and AB divided by the square of the length of AB:
    
					(1)    AC dot AB
							r = ---------  
									||AB||^2
    
					r has the following meaning:
    
							r=0      P = A
							r=1      P = B
							r<0      P is on the backward extension of AB
							r>1      P is on the forward extension of AB
							0<r<1    P is interior to AB
    
					The length of a line segment in d dimensions, AB is computed by:
    
							L = sqrt( (Bx-Ax)^2 + (By-Ay)^2 + ... + (Bd-Ad)^2)

					so in 2D:  
    
							L = sqrt( (Bx-Ax)^2 + (By-Ay)^2 )
    
					and the dot product of two vectors in d dimensions, U dot V is computed:
    
							D = (Ux * Vx) + (Uy * Vy) + ... + (Ud * Vd)
    
					so in 2D:  
    
							D = (Ux * Vx) + (Uy * Vy) 
    
					So (1) expands to:
    
									(Cx-Ax)(Bx-Ax) + (Cy-Ay)(By-Ay)
							r = -------------------------------
																L^2

					The point P can then be found:

							Px = Ax + r(Bx-Ax)
							Py = Ay + r(By-Ay)

					And the distance from A to P = r*L.

					Use another parameter s to indicate the location along PC, with the 
					following meaning:
								s<0      C is left of AB
								s>0      C is right of AB
								s=0      C is on AB

					Compute s as follows:

									(Ay-Cy)(Bx-Ax)-(Ax-Cx)(By-Ay)
							s = -----------------------------
															L^2


					Then the distance from C to P = |s|*L.

			*/


			double r_numerator = (cx - ax) * (bx - ax) + (cy - ay) * (by - ay);
			double r_denomenator = (bx - ax) * (bx - ax) + (by - ay) * (by - ay);
			double r = r_numerator / r_denomenator;
			//
			double px = ax + r * (bx - ax);
			double py = ay + r * (by - ay);
			//    
			double s = ((ay - cy) * (bx - ax) - (ax - cx) * (by - ay));

			double distanceLine = Math.Abs(s) / Math.Sqrt(r_denomenator);

			if (distanceLine > MIN_DISTANCE) return false; // if distance to line is over MIN_DISTANCE, it is far enough to be ignored

			return ((r > 0) && (r < 1)); // If outside the segment, it doesn't block the LOS. Otherwise, as DistanceLine <= MIN_DISTANCE, it does       
		}


		/// <summary>
		/// Find the cell in the middle of several cells (barycenter)
		/// </summary>
		/// <param name="alliesCells"></param>
		/// <param name="foesCells"></param>
		/// <returns></returns>
		public int MiddleCell(int[] cellIds)
		{
			int CumulX = 0, CumulY = 0;
			foreach (int cell in cellIds)
			{
				CumulX += xfromid(cell);
				CumulY += yfromid(cell);
			}
			return GetIdFromPosSafe((CumulX + cellIds.Length / 2) / cellIds.Length, (CumulY + cellIds.Length / 2) / cellIds.Length);
		}

		/// <summary>
		/// Return the "flight distance" between to cells. It gives a rought indication on how far they are, 
		/// without processing full PathFinding.
		/// </summary>
		/// <param name="StartCell"></param>
		/// <param name="EndCell"></param>
		/// <param name="iscombatMap"></param>
		/// <returns></returns>
		public double GetFlightDistance(int startCell, int endCell, bool fourDirections = false)
		{
			int dx = Math.Abs(xfromid(startCell) - xfromid(endCell));
			int dy = Math.Abs(yfromid(startCell) - yfromid(endCell));
			if (fourDirections)
				return dx + dy;
			if (dx > dy)
				return dy * 1.414 /* diagonale part */ + /* straight line part */ dx - dy;
			else
				return dx * 1.414 /* diagonale part */ + /* straight line part */ dy - dx;			
		}

		// Returns true when both cells are on the same horizontal or vertical line
		public bool AreInLine(int cellId1, int cellId2)
		{
			return (xfromid(cellId1) == xfromid(cellId2)) || (yfromid(cellId1) == yfromid(cellId2));
		}

		public DirectionsEnum GetOrientation(int cellStart, int cellDest)
		{
			int dx = xfromid(cellDest) - xfromid(cellStart);
			int dy = yfromid(cellDest) - yfromid(cellStart);
			if (dx == 0)
				if (dy == 0) { 
					Debug.Assert(false); 
					return DirectionsEnum.DIRECTION_EAST; 
				} // 0,0 - no mouvement :p
				else
					if (dy < 0) return DirectionsEnum.DIRECTION_SOUTH_WEST; // 0,-1
					else
						return DirectionsEnum.DIRECTION_NORTH_EAST; // 0,1
			else
				if (dx < 0)
					if (dy == 0) return DirectionsEnum.DIRECTION_NORTH_WEST; // -1,0
					else
						if (dy < 0) return DirectionsEnum.DIRECTION_WEST; // -1,-1
						else
							return DirectionsEnum.DIRECTION_NORTH; // -1,1
				else
					if (dy == 0) return DirectionsEnum.DIRECTION_SOUTH_EAST; // 1,0
					else
						if (dy < 0) return DirectionsEnum.DIRECTION_SOUTH; // 1,-1
						else
							return DirectionsEnum.DIRECTION_EAST; // 1,1
		}
	}
}
