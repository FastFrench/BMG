using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using BMG_Structures;
using BMG_Structures.Common;

namespace BMG_IA.PathFinder
{
	public class Pathfinder
	{
		
		// When in combat, then directions are resticted to 4, and distances are manhattan distances
		private bool isInFight { get; set; }

		// Cells stores information about each square.
		private PathFinderMap cells { get; set; }

		// Ctor : provides cells array and mode (combat or not)
		public Pathfinder(CellBase[] _cells, int width, int height, bool eightDirs = false)
		{
			this.cells = new PathFinderMap(width, height, eightDirs);
			// Todo: initialize the internal Map from _cells
		}

		

		// 

		/// <summary>
		/// Reset old PathFinding path from the cells.
		/// </summary>
		public void ClearLogic()
		{
			// Reset some information about the cells.
			cells.ClearLogic();
			
			if (PathResult == null)
				PathResult = new List<int>();
			else
				PathResult.Clear();
			
			StartingCell = Map.CELL_ERROR;
			ExitCell = Map.CELL_ERROR;
		}

		public List<int> PathResult { get; private set; }

		//public Dictionary<int, CellInfo> StartingCells;
		//public Dictionary<int, CellInfo> ExitCells;
		public int StartingCell { get; private set; }
		public int ExitCell { get; private set; }

		#region SubArea filler
		// Used to find next unset closed area in World map and tag all corresponding cells
		//private bool FindNextSubArea(byte SubMapNo)
		//{
		//	int? FirstCellId = null;
		//	foreach (var cell in cells)
		//	{
		//		if (cell.subMapId == 0 && SquareOpen(cell))
		//		{
		//			FirstCellId = cell.cellId;
		//			break;
		//		}
		//	}
		//	if (!FirstCellId.HasValue) return false;
		//	FindPath(new int[] { FirstCellId.Value }, null, false, true); // Only 1st step
		//	bool otherSubAreaToDetect = false;
		//	foreach (var cell in cells)
		//	{
		//		// Mark each cell accessible from starting one
		//		if (cell.DistanceSteps != CellInfo.DEFAULT_DISTANCE)
		//			cell.subMapId = SubMapNo;
		//		else
		//			if (cell.subMapId == 0 && SquareOpen(cell))
		//				otherSubAreaToDetect = true;
		//	}
		//	return otherSubAreaToDetect;
		//}

		//// Identify each unlinked 'submaps' (sets of cells that are not linked together)  
		//public byte SubMapFiller()
		//{

		//	// Reset SubArea data 
		//	foreach (var cell in cells)
		//	{
		//		// Mark each cell as from unset subarea
		//		cell.subMapId = 0;
		//	}

		//	byte SubAreaNo = 0;
		//	while (FindNextSubArea(++SubAreaNo)) ;
		//	return SubAreaNo;
		//}
		#endregion

		#region FinPath algorithm itself

		/// <summary>
		/// Entry point for PathFinding algorithm, with one starting cell, and one exit
		/// </summary>
		public bool FindPath(int StartingCell, int ExitCell)
		{
			int[] StartingCells = new int[] { StartingCell };
			int[] ExitCells = new int[] { ExitCell };
			return FindPath(StartingCells, ExitCells, false);
		}

		/// <summary>
		/// Entry point for PathFinding algorithm, with on starting cell and several exits
		/// </summary>
		public bool FindPath(int StartingCell, int[] ExitCells, bool SelectFartherCells = false)
		{
			int[] StartingCells = new int[] { StartingCell };
			return FindPath(StartingCells, ExitCells, SelectFartherCells);
		}

		/// <summary>
		/// Entry point for PathFinding algorithm, with several starting cells, and one exit
		/// </summary>
		public bool FindPath(int[] StartingCells, int ExitCell)
		{
			int[] ExitCells = new int[] { ExitCell };
			return FindPath(StartingCells, ExitCells, false);
		}

		/// <summary>
		/// Flee away from a set of foes, starting on a given cell
		/// * REVERSED PATH FINDING *
		/// </summary>
		public bool FleeFromFoes(int MyCell, int[] FoeCells, int distance)
		{
			// Set all Foes as starting points, Exit cell as exit (not used anyway in part 1 of PathFinding)
			int[] ExitCells = new int[] { MyCell };
			FindPath(FoeCells, ExitCells, false, true);
			// Step 2
			ExitCell = ExitCells[0];
			int CurrentCell = ExitCell;
			PathResult.Add(ExitCell);
			cells.PathFinderCells[ExitCell].IsInPath = true;
			int NbStepLeft = distance;
			while (NbStepLeft-- > 0)
			{
				// Look through each direction and find the square
				// with the lowest number of steps marked.
				int highestPoint = Map.CELL_ERROR;
				int PreviousDistance = cells.PathFinderCells[CurrentCell].DistanceSteps;
				int highest = PreviousDistance;
				foreach (int newCellId in cells.ValidMoves(CurrentCell))
				{
					int count = cells.PathFinderCells[newCellId].DistanceSteps;
					if (count > highest)
					{
						highest = count;
						highestPoint = newCellId;
					}
				}
				if (highest != PreviousDistance)
				{
					// Mark the square as part of the path if it is the lowest
					// number. Set the current position as the square with
					// that number of steps.
					PathResult.Add(highestPoint);
					cells.PathFinderCells[highestPoint].IsInPath = true;
					CurrentCell = highestPoint;
				}
				else
				{
					// Can't find a longer path => stop now :(
					break;
				}
			}
			PathResult.Reverse(); // Reverse the path, as we started from exit
			return PathResult.Count > 0;
		}

		/// <summary>
		/// PathFinding main method
		/// </summary>
		/// <param name="StartingCells"></param>
		/// <param name="ExitCells"></param>
		/// <param name="SelectFartherCells"></param>
		/// <param name="FirstStepOnly"></param>
		/// <returns></returns>
		public bool FindPath(int[] StartingCells, int[] ExitCells, bool SelectFartherCells = false, bool FirstStepOnly = false)
		{
			Random rnd = new Random();
			ClearLogic();

			if ((StartingCells == null) || (StartingCells.Length == 0)) return false; // We need at least one starting cell
			if (!FirstStepOnly && (ExitCells == null || ExitCells.Length == 0)) return false; // We need at least one exit cell for step 2
			// PC starts at distance of 0. Set 0 to all possible starting cells
			foreach (int cell in StartingCells)
				cells.PathFinderCells[cell].DistanceSteps = 0;
			//    cells[StartingCell].DistanceSteps = 0;
			int NbMainLoop = 0;

			// First try the straight forward way

			// Then kinda traditionnal A* algorithm
			while (true)
			{
				NbMainLoop++;
				bool madeProgress = false;

				// Look at each square on the board.
				for (int cell = 0; cell < cells.TotalSize; cell++)
				{
					// If the square is open, look through valid moves given
					// the coordinates of that cell.
					if (cells.SquareOpen(cell))
					{
						int passHere = cells.PathFinderCells[cell].DistanceSteps;

						foreach (int newCell in cells.ValidMoves(cell))
						{
							int newPass = passHere;

							if (isInFight)
								newPass++;
							else
								newPass += cells.AreInLine(cell, newCell) ? cells[newCell].Weight : (int)(cells[newCell].Weight * 1.414);

							if (cells.PathFinderCells[newCell].DistanceSteps > newPass)
							{
								cells.PathFinderCells[newCell].DistanceSteps = newPass;
								madeProgress = true;
							}
						}
					}
				}
				if (!madeProgress)
				{
					break;
				}
			}

			if (FirstStepOnly)
				return true;
			// Step 2
			// Mark the path from Exit to Starting position.
			// if several Exit cells, then get the lowest distance one = the closest from one starting cell
			// (or the highest distance one if SelectFartherCells)
			ExitCell = ExitCells[0];
			int MinDist = cells.PathFinderCells[ExitCell].DistanceSteps;
			if (SelectFartherCells)
			{
				foreach (int cell in ExitCells)
					if (cells.PathFinderCells[cell].DistanceSteps > MinDist)
					{
						ExitCell = cell;
						MinDist = cells.PathFinderCells[cell].DistanceSteps;
					}
			}
			else
			{
				foreach (int cell in ExitCells)
					if (cells.PathFinderCells[cell].DistanceSteps < MinDist)
					{
						ExitCell = cell;
						MinDist = cells.PathFinderCells[cell].DistanceSteps;
					}
			}
			int CurrentCell = ExitCell;
			PathResult.Add(ExitCell);
			cells.PathFinderCells[ExitCell].IsInPath = true;
			List<int> LowestPoints = new List<int>(10);
			int lowestPoint;
			int lowest;

			while (true)
			{
				// Look through each direction and find the square
				// with the lowest number of steps marked.
				lowestPoint = Map.CELL_ERROR;
				lowest = CellInfo.DEFAULT_DISTANCE;

				foreach (int newCell in cells.ValidMoves(CurrentCell))
				{
					int count = cells.PathFinderCells[newCell].DistanceSteps;
					if (count < lowest)
					{
						LowestPoints.Clear();
						lowest = count;
						lowestPoint = newCell;
					}
					else
						if (count == lowest)
						{
							if (LowestPoints.Count == 0)
								LowestPoints.Add(lowestPoint);
							LowestPoints.Add(newCell);
						}
				}
				if (lowest == CellInfo.DEFAULT_DISTANCE) break; // Can't find a valid way :(

				if (LowestPoints.Count > 1) // Several points with same distance =>> randomly select one of them
					lowestPoint = LowestPoints[rnd.Next(LowestPoints.Count)];

				// Mark the square as part of the path if it is the lowest
				// number. Set the current position as the square with
				// that number of steps.
				PathResult.Add(lowestPoint);
				cells.PathFinderCells[lowestPoint].IsInPath = true;
				CurrentCell = lowestPoint;


				if (cells.PathFinderCells[CurrentCell].DistanceSteps == 0) // Exit reached            
				{
					StartingCell = CurrentCell;
					// We went from closest Exit to a Starting position, so we're finished.
					break;
				}
			}
			Debug.Assert(CurrentCell == StartingCell);
			return CurrentCell == StartingCell;
		}

		#endregion FinPath algorithm itself


		/// <summary>
		/// Compute the exact length of the last path. 
		/// </summary>
		/// <returns></returns>
		public double GetLengthOfLastPath()
		{
			double distance = 0.0;
			for (int i = 1; i < PathResult.Count; i++)
				distance += cells.GetFlightDistance(PathResult[i - 1], PathResult[i]);
			return distance;
		}

		/// <summary>
		/// Gives first index of the list to be used in order to be as close as possible as MinDistance
		/// </summary>
		/// <param name="MinDistance"></param>
		/// <returns></returns>
		private int GetIndexForDistance(int MinDistance)
		{
			if (MinDistance <= 0) return 0;
			double distance = 0.0;
			double precDistance = 0.0;
			for (int i = 1; i < PathResult.Count; i++)
			{
				distance += cells.GetFlightDistance(PathResult[i - 1], PathResult[i]);
				if (Math.Abs(distance - MinDistance) > Math.Abs(precDistance - MinDistance)) return i - 1;
				precDistance = distance;
			}
			return PathResult.Count - 1;
		}

		/// <summary>
		/// Remove final steps of the path, so that it ends at MinDistance from target (instead of 0)
		/// Warning : call this only once
		/// </summary>
		/// <param name="MinDistance"></param>
		private void TruncatePathAtMinDistance(int MinDistance)
		{
			if (MinDistance <= 0) return;
			int index = GetIndexForDistance(MinDistance);
			if (index > 0)
				PathResult.RemoveRange(0, index - 1);
		}

		/// <summary>
		/// Returns the last path, unpacked. Remove MinDistance last steps. 
		/// BEWARE : Use this (or GetLastPackedPath) only ONCE
		/// </summary>
		/// <param name="MinDistance"></param>
		/// <returns></returns>
		public int[] GetLastPath(int MinDistance)
		{
			TruncatePathAtMinDistance(MinDistance);
			int[] res = new int[PathResult.Count];
			for (int i = 0; i < PathResult.Count; i++)
				res[i] = PathResult[PathResult.Count - i - 1];
			return res;
		}

		private List<int> PathBackup;
		private void PushCurrentPath()
		{
			PathBackup = new List<int>(PathResult);
		}

		private void PopCurrentPath()
		{
			PathResult = PathBackup;
		}


	}
}
