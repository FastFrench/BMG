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
	public class Map
	{
		private CellBase[,] cells;
		private BattleFieldBase bf;
		public int Width { get; set; }
		public int Height { get; set; }
		public Map(int width, int height, BattleFieldBase _bf)
		{
			Width = width;
			Height = height;
			cells = new CellBase[width, height];
			bf = _bf;
			FillOuterWalls();
		}

		public CellBase this[int x, int y]
		{
			get
			{
				return cells[x, y];
			}
			set
			{
				cells[x, y] = value;
			}
		}

		public CellBase this[Point pt]
		{
			get
			{
				if (pt.IsInDeck)
				{
					Debug.Assert(false);
					return new CellBase();
				}
				return cells[pt.X, pt.Y];
			}
			set
			{
				if (pt.IsInDeck)
				{
					Debug.Assert(false);
					return;
				}
				
				cells[pt.X, pt.Y] = value;
			}
		}

		#region Cells management
		public void FillAllCells(CellBase.CellContent content)
		{
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Width; y++)
					cells[x, y].Content = content;
		}

		public void UpdateCells()
		{
			// Reset troop content: remove all except obstacles (won't change, and if they do, then changes have to update this map)
			for (int x = 0; x < Width; x++)
				for (int y = 0; y < Width; y++)
					cells[x, y].Content &= CellBase.CellContent.AllObstacles;
			
			for (int teamIndex = 0; teamIndex < bf.Teams.Count; teamIndex++)
			{
				TeamBase team = bf.Teams[teamIndex];
				foreach (PlayerBase player in team.Players)
				{
					foreach (TroopBase troop in player.Army.Troops)
						if (!troop.CurrentPosition.IsInDeck)
							cells[troop.CurrentPosition.X, troop.CurrentPosition.Y].Content |= CellBase.GetTroopFlag(teamIndex, troop.Altitude);
					foreach (BuildingBase building in player.OwnBuildings)
						if (!building.CurrentPosition.IsInDeck)
							cells[building.CurrentPosition.X, building.CurrentPosition.Y].Content |= CellBase.GetBuildingFlag(teamIndex, building.Altitude);
				}
			}
		}

		public void SetContent(Point pt, CellBase.CellContent content)
		{
			cells[pt.X, pt.Y].Content = content;
		}

		public void SetOrContent(Point pt, CellBase.CellContent content)
		{
			cells[pt.X, pt.Y].Content |= content;
		}

		public void SetAndContent(Point pt, CellBase.CellContent content)
		{
			cells[pt.X, pt.Y].Content &= content;
		}

		#endregion

		#region obstacles
		public void AddObstacle(Point pt, AltitudeEnum altitude = AltitudeEnum.All)
		{
			CellBase.CellContent obstacles = CellBase.CellContent.Empty;
			if (altitude.HasFlag(AltitudeEnum.Fly)) obstacles |= CellBase.CellContent.FlyObstacle;
			if (altitude.HasFlag(AltitudeEnum.Ground)) obstacles |= CellBase.CellContent.GroundObstacle;
			if (altitude.HasFlag(AltitudeEnum.Underground)) obstacles |= CellBase.CellContent.UnderGroundObstacle;
			SetOrContent(pt, obstacles);
		}

		public void FillOuterWalls()
		{
			for (int x = 0; x < Width; x++)
			{
				cells[x, 0].Content = CellBase.CellContent.AllObstacles;
				cells[x, Height - 1].Content = CellBase.CellContent.AllObstacles;
			}
			for (int y = 0; y < Height; y++)
			{
				cells[0, y].Content = CellBase.CellContent.AllObstacles;
				cells[Width-1, y].Content = CellBase.CellContent.AllObstacles;
			}
		}
		#endregion obstacles

	}
}
