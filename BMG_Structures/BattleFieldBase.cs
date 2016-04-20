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
			Cells = new CellBase[Width,Height];
			return true;
		}

		// Reasonnably fast method that returns all points at range
		public IEnumerable<Point> CellsAtRange(Point point, int maxRange, int minRange = 0, CellBase.CellContent? cellContentMask = null )
		{
			int minRange2 = minRange * minRange;
			int maxRange2 = maxRange * maxRange;
			for (int x = point.X - maxRange; x <= point.X + maxRange; x++)
				if (x>=0 && x<Width)
					for (int y = point.Y - maxRange; y<=point.Y + maxRange; y++)
						if (y>=0 && y<Height)
						{
							if (cellContentMask != null && (Cells[x,y].Content & cellContentMask.Value) == CellBase.CellContent.Empty) 
								continue;
							int d2 = (x - point.X) * (x - point.X) + (y - point.Y) * (y - point.Y);
							if (d2 <= maxRange2 && d2 >= minRange2)
								yield return new Point(x, y);
						}
		}
		
		public IEnumerable<PlaceableBase> GetPlaceablesInCell(Point point, bool includeBuildings = true, bool includeTroops = true, int? teamId = null)
		{
			return	
				Teams.Where(te=>teamId == null || te.TeamId != teamId.Value).Select(te=>te.Players).
					Aggregate<IEnumerable<PlayerBase>>((lp, next)=>lp.Concat(next)).
						Aggregate<PlayerBase, IEnumerable<PlaceableBase>>(
							Enumerable.Empty<PlaceableBase>(), 
							(current, next) => current.Concat(next.Army.Troops.Concat<PlaceableBase>(next.OwnBuildings).Where(pl=>pl.CurrentPosition == point))
							);
		}
		
		// Returns all placeables (buildings or troops) at range
		public IEnumerable<PlaceableBase> PlaceablesAtRange(Point point, int maxRange, int minRange = 0, bool includeBuildings = true, bool includeTroops = true, int? teamId = null)
		{
			int minRange2 = minRange * minRange;
			int maxRange2 = maxRange * maxRange;
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
			return
				CellsAtRange(point, maxRange, minRange, cellContentMask).
					Select(pt => GetPlaceablesInCell(pt, includeBuildings, includeTroops, teamId)).
						Aggregate<IEnumerable<PlaceableBase>>((current, next) => current.Concat(next));
		}

		// From the closest to the farest
		public IEnumerable<PlaceableBase> SortedPlaceablesAtRange(Point point, int maxRange, int minRange = 0, bool includeBuildings = true, bool includeTroops = true, int? teamId = null)
		{
			return PlaceablesAtRange(point, maxRange, minRange, includeBuildings, includeTroops, teamId).OrderBy(pl => pl.CurrentPosition.SquareDistance(point));
		}


		public void UpdateCells()
		{
			// Reset troop content
			foreach (CellBase cell in Cells)
				cell.Content &= CellBase.CellContent.AllExceptTroops;
					
			for (int teamIndex = 0; teamIndex < Teams.Length; teamIndex++)
			{
				TeamBase team = Teams[teamIndex];
				foreach(PlayerBase player in team.Players)
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
	}
}
