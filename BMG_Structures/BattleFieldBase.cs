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
	public class BattleFieldBase
	{
		public int Width { get; private set; }
		public int Height { get; private set; }

		public TeamBase[] Teams { get; set; }
		public CellBase[,] Cells { get; set; }


		public virtual bool Initialize(int width, int height)
		{
			Width = width;
			Height = height;
			if (Width < 2 || Height < 2) return false;
			Cells = new CellBase[Width, Height];
			rnd = new Random();
			return true;
		}

		#region Cells management
		public void UpdateCells()
		{
			// Reset troop content
			foreach (CellBase cell in Cells)
				cell.Content &= CellBase.CellContent.AllExceptTroops;

			for (int teamIndex = 0; teamIndex < Teams.Length; teamIndex++)
			{
				TeamBase team = Teams[teamIndex];
				foreach (PlayerBase player in team.Players)
				{
					foreach (TroopBase troop in player.Army.Troops)
						if (!troop.CurrentPosition.IsInDeck)
							Cells[troop.CurrentPosition.X, troop.CurrentPosition.Y].Content |= CellBase.GetTroopFlag(teamIndex);
					foreach (BuildingBase building in player.OwnBuildings)
						if (!building.CurrentPosition.IsInDeck)
							Cells[building.CurrentPosition.X, building.CurrentPosition.Y].Content |= CellBase.GetBuildingFlag(teamIndex);
				}
			}
		}
		#endregion

		#region Search routines
		// Reasonnably fast method that returns all points at range
		public IEnumerable<Point> CellsAtRange(Point point, int maxRange, int minRange = 0, CellBase.CellContent? cellContentMask = null)
		{
			int minRange2 = minRange * minRange;
			int maxRange2 = maxRange * maxRange;
			for (int x = point.X - maxRange; x <= point.X + maxRange; x++)
				if (x >= 0 && x < Width)
					for (int y = point.Y - maxRange; y <= point.Y + maxRange; y++)
						if (y >= 0 && y < Height)
						{
							if (cellContentMask != null && (Cells[x, y].Content & cellContentMask.Value) == CellBase.CellContent.Empty)
								continue;
							int d2 = (x - point.X) * (x - point.X) + (y - point.Y) * (y - point.Y);
							if (d2 <= maxRange2 && d2 >= minRange2)
								yield return new Point(x, y);
						}
		}

		/// <summary>
		/// Retrieves all placeables (troops and/or buildings) in a given Cell. Can possibly exclude own team.
		/// </summary>
		/// <param name="point"></param>
		/// <param name="includeBuildings"></param>
		/// <param name="includeTroops"></param>
		/// <param name="excludedTeamId"></param>
		/// <returns></returns>
		public IEnumerable<PlaceableBase> GetPlaceablesInCell(Point point, bool includeBuildings = true, bool includeTroops = true, int? excludedTeamId = null)
		{
			return
				Teams.Where(te => excludedTeamId == null || te.TeamId != excludedTeamId.Value).Select(te => te.Players).
					Aggregate<IEnumerable<PlayerBase>>((lp, next) => lp.Concat(next)).
						Aggregate<PlayerBase, IEnumerable<PlaceableBase>>(
							Enumerable.Empty<PlaceableBase>(),
							(current, next) => current.Concat(next.Army.Troops.Concat<PlaceableBase>(next.OwnBuildings).Where(pl => pl.CurrentPosition == point))
							);
		}

		private CellBase.CellContent contentMask(bool includeBuildings = true, bool includeTroops = true, int? teamId = null)
		{
			CellBase.CellContent cellContentMask = CellBase.CellContent.Empty;
			if (includeBuildings)
			{
				cellContentMask |= CellBase.CellContent.BuildingMask;
				if (teamId != null)
					cellContentMask ^= CellBase.GetBuildingFlag(teamId.Value);
			}
			if (includeTroops)
			{
				cellContentMask |= CellBase.CellContent.TroopMask;
				if (teamId != null)
					cellContentMask ^= CellBase.GetTroopFlag(teamId.Value);
			}
			return cellContentMask;
		}

		// Returns all placeables (buildings or troops) at range
		public IEnumerable<PlaceableBase> PlaceablesAtRange(Point point, int maxRange, int minRange = 0, bool includeBuildings = true, bool includeTroops = true, int? teamId = null)
		{
			int minRange2 = minRange * minRange;
			int maxRange2 = maxRange * maxRange;
			CellBase.CellContent cellContentMask = contentMask(includeBuildings, includeTroops, teamId);
			return
				CellsAtRange(point, maxRange, minRange, cellContentMask).
					Select(pt => GetPlaceablesInCell(pt, includeBuildings, includeTroops, teamId)).
						Aggregate<IEnumerable<PlaceableBase>>((current, next) => current.Concat(next));
		}

		// Same as PlaceablesAtRange, but ordered from the closest to the farthest
		public IEnumerable<PlaceableBase> SortedPlaceablesAtRange(Point point, int maxRange, int minRange = 0, bool includeBuildings = true, bool includeTroops = true, int? teamId = null)
		{
			return PlaceablesAtRange(point, maxRange, minRange, includeBuildings, includeTroops, teamId).OrderBy(pl => pl.CurrentPosition.SquareDistance(point));
		}

		private Random rnd { get; set; }
		// Find the closest cell then contents either troops and/or buildings
		// If random = true, then will take randomly one of all the possible shortest range cells. Otherwise, will take the first one (left=>right, top=>down). 
		public Point FindClosestCellInRange(Point point, int maxRange, int minRange = 0, bool includeBuildings = true, bool includeTroops = true, int? teamId = null, bool random = false)
		{
			CellBase.CellContent cellContentMask = contentMask(includeBuildings, includeTroops, teamId);
			Point bestCellSoFar = Point.InDeck;
			int bestRange2SoFar = int.MaxValue;
			int minRange2 = minRange * minRange;
			int maxRange2 = maxRange * maxRange;
			List<Point> candidates = null;
			for (int x = point.X - maxRange; x <= point.X + maxRange; x++)
				if (x >= 0 && x < Width)
					for (int y = point.Y - maxRange; y <= point.Y + maxRange; y++)
						if (y >= 0 && y < Height)
						{
							if ((Cells[x, y].Content & cellContentMask) == CellBase.CellContent.Empty)
								continue;
							int d2 = (x - point.X) * (x - point.X) + (y - point.Y) * (y - point.Y);
							if (d2 <= maxRange2 && d2 >= minRange2)
							{
								if (d2 < bestRange2SoFar)
								{
									bestRange2SoFar = d2;
									bestCellSoFar = new Point(x, y);
									candidates = null;
								}
								else
									if (random && d2 == bestRange2SoFar)
									{
										if (candidates == null)
											candidates = new List<Point>();
										candidates.Add(bestCellSoFar);
										candidates.Add(new Point(x, y));
									}
							}
						}
			if (candidates != null)
				return candidates[rnd.Next(candidates.Count)];
			return bestCellSoFar;
		}
		#endregion Search routines

	}
}
