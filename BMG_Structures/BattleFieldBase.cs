using System;
using System.Collections.Generic;
using System.Diagnostics;
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

		public List<TeamBase> Teams { get; set; }
		public Map Cells { get; set; }


		public virtual bool Initialize(int width, int height)
		{
			Width = width;
			Height = height;
			if (Width < 2 || Height < 2) return false;
			Cells = new Map(Width, Height, this);
			rnd = new Random();
			Teams = new List<TeamBase>();
			return true;
		}

		#region Search routines
		// Reasonnably fast method that returns all points at range
		public IEnumerable<Point> CellsAtRange(Point point, int maxRange, int minRange = 0, CellBase.CellContent? needAtLeastOneBitFromThisMask = null)
		{
			int minRange2 = minRange * minRange;
			int maxRange2 = maxRange * maxRange;
			for (int x = point.X - maxRange; x <= point.X + maxRange; x++)
				if (x >= 0 && x < Width)
					for (int y = point.Y - maxRange; y <= point.Y + maxRange; y++)
						if (y >= 0 && y < Height)
						{
							if (needAtLeastOneBitFromThisMask != null && (Cells[x, y].Content & needAtLeastOneBitFromThisMask.Value) == CellBase.CellContent.Empty)
								continue;
							int d2 = (x - point.X) * (x - point.X) + (y - point.Y) * (y - point.Y);
							if (d2 <= maxRange2 && d2 >= minRange2)
								yield return new Point(x, y);
						}
		}

		public IEnumerable<PlayerBase> GetAllPlayers(int? excludedTeamId = null)
		{
			return Teams.Where(te => excludedTeamId == null || te.TeamId != excludedTeamId.Value).Select(te => te.Players).
								Aggregate<IEnumerable<PlayerBase>>((lp, next) => lp.Concat(next));
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
				GetAllPlayers(excludedTeamId).
						Aggregate<PlayerBase, IEnumerable<PlaceableBase>>(
							Enumerable.Empty<PlaceableBase>(),
							(list, player) => list.Concat(player.GetPlaceables(includeBuildings, includeTroops).Where(pl => pl.CurrentPosition == point))
							);
		}

		private CellBase.CellContent contentMask(bool includeBuildings = true, bool includeTroops = true, int? teamId = null)
		{
			CellBase.CellContent cellContentMask = CellBase.CellContent.Empty;
			if (includeBuildings)
			{
				cellContentMask |= CellBase.CellContent.BuildingMask;
				if (teamId != null)
					cellContentMask ^= CellBase.GetBuildingFlag(teamId.Value, AltitudeEnum.Underground) | CellBase.GetBuildingFlag(teamId.Value, AltitudeEnum.Fly) | CellBase.GetBuildingFlag(teamId.Value, AltitudeEnum.Ground);
			}
			if (includeTroops)
			{
				cellContentMask |= CellBase.CellContent.TroopMask;
				if (teamId != null)
					cellContentMask ^= CellBase.GetTroopFlag(teamId.Value, AltitudeEnum.Underground) | CellBase.GetTroopFlag(teamId.Value, AltitudeEnum.Fly) | CellBase.GetTroopFlag(teamId.Value, AltitudeEnum.Ground);
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
		
		/// <summary>
		/// Find the closest cell then contents either troops and/or buildings
		/// </summary>
		/// <param name="point">Location from which we are looking for a proper spot un range</param>
		/// <param name="maxRange"></param>
		/// <param name="minRange"></param>
		/// <param name="includeBuildings"></param>
		/// <param name="includeTroops"></param>
		/// <param name="teamId">When provided (not null), will ignore all elements from that team.</param>
		/// <param name="random">If random = true, then will take randomly one of all the possible shortest range cells. Otherwise, will take the first one (left=>right, top=>down). </param>
		/// <param name="refusedCellsMask">Each bit from this mask should be 0 in the cell.Content. Can be used for instance to find a cell that has no obstacle for ground move. </param>
		/// <param name="neededCellsMask">At least one of those bits should be 1 in the cell.Content. Can be used for instance to find a cell with a flying or ground target. </param>
		/// <returns></returns>
		public Point FindClosestCellInRange(Point point, int maxRange, int minRange = 0, bool includeBuildings = true, bool includeTroops = true, int? teamId = null, bool random = false, CellBase.CellContent refusedCellsMask = CellBase.CellContent.Empty, CellBase.CellContent? neededCellsMask = null)
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
							if ((refusedCellsMask & Cells[x, y].Content) != CellBase.CellContent.Empty)
								continue;
							if (neededCellsMask != null && ((neededCellsMask & Cells[x, y].Content) == CellBase.CellContent.Empty))
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

		/// <summary>
		/// Find a possible target as close as possible for the source troop or building
		/// </summary>
		/// <param name="source"></param>
		/// <param name="includeBuildings"></param>
		/// <param name="includeTroops"></param>
		/// <param name="random"></param>
		/// <returns></returns>
		public PlaceableBase FindClosestTargetInRange(PlaceableBase source, bool includeBuildings = true, bool includeTroops = true, bool random = false)
		{
			var targetMask = source.TargetableAltitudes;
			Point bestCell = FindClosestCellInRange(source.CurrentPosition, source.MaxAttackRange, source.MinAttackRange, includeBuildings, includeTroops, source.Player.TeamId, random, CellBase.CellContent.Empty, source.TargetMask(includeTroops, includeBuildings));
			var placeables = GetPlaceablesInCell(bestCell, includeBuildings, includeTroops, source.Player.TeamId).Where(pl=>(pl.Altitude & source.TargetableAltitudes) != 0).ToArray();
			if (placeables == null || placeables.Length==0) return null;
			if (!random)
				return placeables[0];
			return placeables[rnd.Next(placeables.Length)];
		}
		#endregion Search routines
		
		public Point RandomPoint()
		{
			return new Point(1+rnd.Next(Width-2), 1+rnd.Next(Height-2));
		}


		public Point SelectARandomDestination(TroopBase troop)
		{
			Point res = Point.InDeck;
			CellBase.CellContent mask = troop.ImpossibleMoveMask();
			int timeout = 100;
			do
			{
				res = RandomPoint();
				if (timeout-- == 0)
				{
					res = Point.InDeck;
					Debug.Assert(false, "Can't find an accessible cell out of obstacles");
					break;
				}
			} while ((Cells[res.X, res.Y].Content & mask) != CellBase.CellContent.Empty);
			return res;
		}
	}
}
