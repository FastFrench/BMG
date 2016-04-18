using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BMG_Structures.Common;
using BMG_Structures.Troops;

namespace BMG_Structures
{
	public class BattleFieldBase
	{
		public int Width { get; set; }
		public int Height { get; set; }

		public TeamBase[] Teams { get; set; }
		public CellBase[,] Cells { get; set; }


		virtual bool Initialize()
		{
			if (Width < 2 || Height < 2) return false;
			Cells = new CellBase[Width,Height];
			return true;
		}

		// Reasonnably fast method that returns all points at range
		IEnumerable<Point> CellsAtRange(Point point, int maxRange, int minRange = 0)
		{
			int minRange2 = minRange * minRange;
			int maxRange2 = maxRange * maxRange;
			for (int x = point.X - maxRange; x <= point.X + maxRange; x++)
				if (x>=0 && x<Width)
					for (int y = point.Y - maxRange; y<=point.Y + maxRange; y++)
						if (y>=0 && y<Height)
						{
							int d2 = (x - point.X) * (x - point.X) + (y - point.Y) * (y - point.Y);
							if (d2 <= maxRange2 && d2 >= minRange2)
								yield return new Point(x, y);
						}
		}

		void UpdateCells()
		{
			// Reset troop content
			foreach (CellBase cell in Cells)
				cell.Content &= CellBase.CellContent.AllExceptTroops;
					
			for (int teamIndex = 0; teamIndex < Teams.Length; teamIndex++)
			{
				//Teams[teamIndex].
				//Array.ForEach(Cells, cellRange => Array.ForEach(cellRange, cell => cell.Content &= Cell.CellContent.AllExceptTroops));
			}


		}
	}
}
